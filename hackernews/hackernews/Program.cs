using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Xml.Linq;
using hackernews.Entities;
using hackernews.Classes;
using Newtonsoft.Json.Linq;

namespace hackernews
{
    class Program
    {
        static void Main(string[] args)
        {     
            int nuberOfPostRequested = 0;
            //args = new string[] { "--posts", "32" };

            if (args.Count() > 1 && !string.IsNullOrEmpty(args[0]) && args[0].Equals("--posts") && int.TryParse(args[1], out nuberOfPostRequested) && nuberOfPostRequested <= 100)
            {
                string xpathPostFilter = "//table[@class=\"itemlist\"][1]/tr";
                Scraper scraper = new Scraper("https://news.ycombinator.com/news", nuberOfPostRequested, xpathPostFilter);
                List<Post> posts = scraper.parse();
                string jsonFormatted = JValue.Parse(JsonConvert.SerializeObject(posts)).ToString(Formatting.Indented);

                Console.WriteLine(jsonFormatted);
            }
            else
            {
                Console.WriteLine("========================[ USAGE ]==========================\n\n");
                Console.WriteLine("hackernews.exe --posts n\n");
                Console.WriteLine("---------------------------------------\n");
                Console.WriteLine("n = Number of posts requested ( less or equals to 100)\n\n");
            }           

        }


    }
}
