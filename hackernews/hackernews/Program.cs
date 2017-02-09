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
using hackernews.Interfaces;

namespace hackernews
{
    class Program
    {
        static void Main(string[] args)
        {     
            int nuberOfPostRequested = 0;
            
            // check that the inputs are respecting the requirement
            // inputs format: --posts n
            // n : maximun output post (<= 100)
            if (args.Count() > 1 && !string.IsNullOrEmpty(args[0]) && args[0].Equals("--posts") && int.TryParse(args[1], out nuberOfPostRequested) && nuberOfPostRequested <= 100)
            {
                // initilize the scraping
                ILauncher start = new HackerNewsLauncher();
                 
                // start scraping the html document
                List<Post> posts = start.getPosts(nuberOfPostRequested);

                // formating the output
                string jsonFormatted = JValue.Parse(JsonConvert.SerializeObject(posts)).ToString(Formatting.Indented);

                // output the posts
                Console.WriteLine("\n"+jsonFormatted);
            }

            // otherwise output usage
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
