using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using hackernews.Classes;

namespace hackernewsTests
{
    [TestClass]
    public class hackernewsUnitTest
    {
        [TestMethod]
        public void OnePostRequestedTest()
        {
            int nuberOfPostRequested = 1;
            string xpathPostFilter = "//table[@class=\"itemlist\"][1]/tr";
            Scraper scraper = new Scraper("https://news.ycombinator.com/news", nuberOfPostRequested, xpathPostFilter);

            Assert.AreEqual(scraper.parse().Count, nuberOfPostRequested);
        }

        [TestMethod]
        public void thirtyFivePostsRequestedTest()
        {
            int nuberOfPostRequested = 35;
            string xpathPostFilter = "//table[@class=\"itemlist\"][1]/tr";
            Scraper scraper = new Scraper("https://news.ycombinator.com/news", nuberOfPostRequested, xpathPostFilter);

            Assert.AreEqual(scraper.parse().Count, nuberOfPostRequested);
        }
    }
}
