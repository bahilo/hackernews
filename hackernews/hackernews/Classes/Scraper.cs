using hackernews.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews.Classes
{
    public class Scraper
    {
        const int _numberOfPostOnEachPage = 30;
        string _uri;
        List<Post> _posts;
        string _xpathPostFilter;
        int _numberOfPostsRequested;

        public Scraper(string webPageUri, int numberOfPostsRequested, string xpathPostFilter)
        {
            _uri = webPageUri;
            _posts = new List<Post>();
            _numberOfPostsRequested = numberOfPostsRequested;
            _xpathPostFilter = xpathPostFilter;
       }
        
        public List<Post> parse()
        {
            List<Post> outPutPosts = new List<Post>();
            int nbPageToParse = (_numberOfPostsRequested > _numberOfPostOnEachPage) ? (int)Math.Ceiling((double) _numberOfPostsRequested / _numberOfPostOnEachPage) : 1 ;
            
            for(int page = 1; page <= nbPageToParse; page++)
            {
                try
                {
                    outPutPosts = outPutPosts.Concat(readDocument(page, _numberOfPostsRequested - outPutPosts.Count )).ToList();
                }
                catch (ApplicationException)
                {
                    break;
                }
            }
            return outPutPosts;
        }

        private List<Post> readDocument( int page, int numberOfPostRemain)
        {
            List<Post> outPutPosts = new List<Post>();
            var pageContent = new HtmlWeb().Load(_uri + "?p=" + page);
            if (pageContent.DocumentNode.SelectNodes(_xpathPostFilter) != null)
            {
                int nbAssignedProperties = 0;
                Post post = new Post();
                bool areAllPostPropertiesAssigned = false;
                int numberOfPosts = pageContent.DocumentNode.SelectNodes(_xpathPostFilter).Count();

                for (int i = 1; i <= numberOfPosts; i++)
                {
                    string xpathPostElementFiler = _xpathPostFilter + "[" + i + "]/td";
                    if (pageContent.DocumentNode.SelectNodes(xpathPostElementFiler) != null)
                    {
                        nbAssignedProperties++;
                        int nuberOfElementByPost = pageContent.DocumentNode.SelectNodes(xpathPostElementFiler).Count();

                        for (int y = 1; y <= nuberOfElementByPost; y++)
                        {
                            string xpathPostEmentChildFiler = xpathPostElementFiler + "[" + y + "]";
                            if (pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[@class=\"storylink\"]") != null)
                            {
                                post.Title = pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[@class=\"storylink\"]").InnerText;
                                post.Uri = pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[@class=\"storylink\"]").Attributes["href"].Value;
                            }

                            if (pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//span[@class=\"score\"]") != null)
                            {
                                post.Points = Utility.intTryParse(pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//span[@class=\"score\"]").InnerText.Split(' ')[0]);
                            }
                            if (pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[3]") != null)
                            {
                                string strNbComment = pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[3]").InnerText;
                                post.Comments = Utility.intTryParse((strNbComment.Split('&').Count() > 0) ? strNbComment.Split('&')[0] : "0");
                            }
                            if (pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[@class=\"hnuser\"]") != null)
                            {
                                post.Author = pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//a[@class=\"hnuser\"]").InnerText;
                            }
                            if (pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//span[@class=\"rank\"]") != null)
                            {
                                post.Rank = Utility.intTryParse(pageContent.DocumentNode.SelectSingleNode(xpathPostEmentChildFiler + "//span[@class=\"rank\"]").InnerText.Split('.')[0]);
                            }

                            if (nbAssignedProperties == 2)
                                areAllPostPropertiesAssigned = true;
                        }

                        if (areAllPostPropertiesAssigned)
                        {
                            outPutPosts.Add(post);
                            post = new Post();
                            areAllPostPropertiesAssigned = false;
                            if (outPutPosts.Count == numberOfPostRemain)
                                return outPutPosts;

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
