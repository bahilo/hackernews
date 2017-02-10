using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using hackernews.Classes;
using hackernews.Interfaces;
using hackernews.Entities;
using System.Collections.Generic;

namespace hackernewsTests
{
    [TestClass]
    public class hackernewsUnitTest
    {
        [TestMethod]
        public void OnePostRequestedTest()
        {
            int nuberOfPostRequested = 1;
            ILauncher start = new FakeHackerLauncher();
            start.initialize();
            Assert.AreEqual(start.getPosts(nuberOfPostRequested).Count, nuberOfPostRequested);
        }

        [TestMethod]
        public void thirtyFivePostsRequestedTest()
        {
            int nuberOfPostRequested = 35;
            ILauncher start = new FakeHackerLauncher();
            start.initialize();
            Assert.AreEqual(start.getPosts(nuberOfPostRequested).Count, nuberOfPostRequested);
        }
    }

    public class FakeHackerLauncher : ILauncher
    {
        IScraper _scraper;
        string _xpathPostFilter = "//table[@class=\"itemlist\"][1]/tr";

        public List<Post> getPosts(int nuberOfPostRequested)
        {
            return _scraper.parse(nuberOfPostRequested);
        }

        public void initialize()
        {
            _scraper = new Scraper("https://news.ycombinator.com/news", _xpathPostFilter);
        }
    }
}
