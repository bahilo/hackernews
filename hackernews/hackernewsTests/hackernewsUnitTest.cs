using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using hackernews.Classes;
using hackernews.Interfaces;

namespace hackernewsTests
{
    [TestClass]
    public class hackernewsUnitTest
    {
        [TestMethod]
        public void OnePostRequestedTest()
        {
            int nuberOfPostRequested = 1;
            ILauncher start = new HackerNewsLauncher();            
            Assert.AreEqual(start.getPosts(nuberOfPostRequested).Count, nuberOfPostRequested);
        }

        [TestMethod]
        public void thirtyFivePostsRequestedTest()
        {
            int nuberOfPostRequested = 35;
            ILauncher start = new HackerNewsLauncher();
            Assert.AreEqual(start.getPosts(nuberOfPostRequested).Count, nuberOfPostRequested);
        }
    }
}
