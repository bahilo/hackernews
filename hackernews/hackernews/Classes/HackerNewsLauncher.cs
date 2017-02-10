using hackernews.Entities;
using hackernews.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hackernews.Classes
{
    public class HackerNewsLauncher : ILauncher
    {
        // represent the xpath matching pattern
        const string _xpathPostFilter = "//table[@class=\"itemlist\"][1]/tr";

        int _nuberOfPostRequested;
        IScraper _scraper;
        

        public void initialize()
        {
            try
            {
                string uri = ConfigurationManager.AppSettings["webpageuri"];
                if (Utility.checkIfValidURI(uri))
                {
                    _scraper = new Scraper(uri, _xpathPostFilter);
                }
                else
                    throw new ApplicationException("The URI ("+uri+") found in the configuration file is not a valid URI.");
                   
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine("Error occurred while initializing scraping - " +ex.Message);
                System.Environment.Exit(1);
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred while initializing scraping.");
                System.Environment.Exit(2);
            }
        }

        public List<Post> getPosts(int nuberOfPostRequested)
        {
            _nuberOfPostRequested = nuberOfPostRequested;
            return _scraper.parse(_nuberOfPostRequested);
        }
    }
}
