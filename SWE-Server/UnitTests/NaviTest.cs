using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class NaviTest : BaseTest
    {
        NaviPlugin.NaviPlugin plugin;

        [TestInitialize]
        public void Init()
        {
            plugin = new NaviPlugin.NaviPlugin();
        }

        [TestMethod]
        public void GeneratingNotice()
        {
            plugin.SetGenerating();

            const string requestString = "GET /?action=Navi HTTP/1.1";
            var request = RequestFromString(requestString);

            var data = plugin.CreateProduct(request);
            Assert.AreEqual(200, data.StatusCode);
            // progress bar is only shown if generating
            Assert.IsTrue(Encoding.Default.GetString(data.Content).Contains("progressBarDiv"));
        }
    }
}
