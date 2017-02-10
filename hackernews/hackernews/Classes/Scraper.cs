using hackernews.Entities;
using hackernews.Interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hackernews.Classes
{
    public class Scraper : IScraper
    {
        const int _numberOfPostOnEachPage = 30;
        string _uri;
        List<Post> _posts;
        string _xpathPostFilter;
        int _numberOfPostsRequested;

        public Scraper(string webPageUri, string xpathPostFilter)
        {
            _uri = webPageUri;
            _xpathPostFilter = xpathPostFilter;
        }

        public Scraper(string webPageUri, int numberOfPostsRequested, string xpathPostFilter)
            : this(webPageUri, xpathPostFilter)
        {            
            _posts = new List<Post>();
            _numberOfPostsRequested = numberOfPostsRequested;
        }

        public List<Post> parse()
        {
            List<Post> outPutPosts = new List<Post>();

            // calcul of the amount of pages to reach the number of posts requested
            int nbPageToParse = (_numberOfPostsRequested > _numberOfPostOnEachPage) ? (int)Math.Ceiling((double)_numberOfPostsRequested / _numberOfPostOnEachPage) : 1;

            // scraping by page
            for (int page = 1; page <= nbPageToParse; page++)
                outPutPosts = outPutPosts.Concat(readDocument(page, _numberOfPostsRequested - outPutPosts.Count)).ToList();

            return outPutPosts;
        }

        public List<Post> parse(int numberOfPostsRequested)
        {
            _numberOfPostsRequested = numberOfPostsRequested;            
            return parse();
        }

        // scraping the html document. 
        // the posts are formated in tables
        // the information of one post is contained on 2 rows of the table
        // then followed by an empty table row
        public List<Post> readDocument( int page, int numberOfPostRemain)
        {
            List<Post> outPutPosts = new List<Post>();

            // regex pattern to meet URI requirement
            string uriPattern = @"^(http://|https://|item\?id=)";
            var regex = new Regex(uriPattern);

            // loading downloading the html document
            var pageContent = new HtmlWeb().Load(_uri + "?p=" + page);

            // checking that the html document matches the xpath pattern
            if (pageContent.DocumentNode.SelectNodes(_xpathPostFilter) != null)
            {
                int nbAssignedProperties = 0;
                Post post = new Post();
                bool areAllPostPropertiesAssigned = false;

                // getting the number of table rows for processing
                int numberOfPosts = pageContent.DocumentNode.SelectNodes(_xpathPostFilter).Count();

                for (int i = 1; i <= numberOfPosts; i++)
                {
                    // getting each piece of information contained in the table row
                    string xpathPostElementFiler = _xpathPostFilter + "[" + i + "]/td";
                    
                    if (pageContent.DocumentNode.SelectNodes(xpathPostElementFiler) != null)
                    {
                        nbAssignedProperties++;
                        int nuberOfElementByPost = pageContent.DocumentNode.SelectNodes(xpathPostElementFiler).Count();

                        for (int y = 1; y <= nuberOfElementByPost; y++)
                        {
                            string xpathPostEmentChildFiler = xpathPostElementFiler + "[" + y + "]";

                            // getting the title an the URI by matching the class storylink of the first table row
                            if (pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[@class=\"storylink\"]") != null)
                            {
                                post.Title = pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[@class=\"storylink\"]").InnerText;
                                post.Title = (post.Title.Length > 256) ? post.Title.Substring(0, 256) : post.Title;
                                post.Uri = pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[@class=\"storylink\"]").Attributes["href"].Value;
                                post.Uri = (regex.Match(post.Uri).Success) ? post.Uri : "" ;
                                
                            }

                            // getting the poins by matching the class score of the second table row
                            if (pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//span[@class=\"score\"]") != null)
                            {
                                post.Points = Utility.intTryParse(pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//span[@class=\"score\"]").InnerText.Split(' ')[0]);
                            }

                            // getting the comment by matching the third link of each td container of the second table row
                            if (pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[3]") != null)
                            {
                                string strNbComment = pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[3]").InnerText;
                                post.Comments = Utility.intTryParse((strNbComment.Split('&').Count() > 0) ? strNbComment.Split('&')[0] : "0");
                            }

                            // getting the author by matching hnuser class of the second table row
                            if (pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[@class=\"hnuser\"]") != null)
                            {
                                post.Author = pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[@class=\"hnuser\"]").InnerText;
                                post.Author = (post.Author.Length > 256 ) ? post.Author.Substring(0, 256) : post.Author;
                            }

                            // getting the rank by matching the rank class of the first table row
                            if (pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//span[@class=\"rank\"]") != null)
                            {
                                post.Rank = Utility.intTryParse(pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//span[@class=\"rank\"]").InnerText.Split('.')[0]);
                            }

                            // checking that the information have been read
                            if (nbAssignedProperties == 2 && !string.IsNullOrEmpty(post.Title) && !string.IsNullOrEmpty(post.Author) && !string.IsNullOrEmpty(post.Uri))
                                areAllPostPropertiesAssigned = true;
                        }

                        // saving the complete information of the each post
                        if (areAllPostPropertiesAssigned)
                        {
                            outPutPosts.Add(post);
                            post = new Post();
                            areAllPostPropertiesAssigned = false;

                            // exit when the amount of requested post has been reached
                            if (outPutPosts.Count == numberOfPostRemain)
                                return outPutPosts;

                            // display the progress 
                            Console.Write(".");
                        }

                    }
                    else
                        nbAssignedProperties = 0;
                }
            }
            return outPutPosts;
        }








    }
}
