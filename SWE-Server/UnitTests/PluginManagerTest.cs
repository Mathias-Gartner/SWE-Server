using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interface;
using SWE_Server;
using System.IO;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class PluginManagerTest : BaseTest
    {
        [TestInitialize]
        public void Init()
        {
            // delete old PluginManager instances
            typeof(PluginManager).GetField("Instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, null);
        }

        [TestMethod]
        public void NullRequest()
        {
            var pm = PluginManager.getInstance();
            pm.LoadPlugins();

            var data = pm.HandleRequest(null);
            Assert.AreEqual(400, data.StatusCode);
            Assert.AreEqual(0, data.Content.Length);
        }

        [TestMethod]
        public void NullUrlRequest()
        {
            var request = new Request(new MemoryStream()); // nothing in stream -> Url will be null

            var pm = PluginManager.getInstance();
            pm.LoadPlugins();

            var data = pm.HandleRequest(request);
            Assert.AreEqual(400, data.StatusCode);
            Assert.AreEqual(0, data.Content.Length);
        }

        [TestMethod]
        public void BrokenPlugin()
        {
            const string requestString = "GET /?action=BrokenPlugin HTTP/1.1\n";
            var request = new Request(new MemoryStream(Encoding.Default.GetBytes(requestString))); 
            
            var pm = PluginManager.getInstance();
            pm.PluginList.Add(new BrokenPlugin());

            var data = pm.HandleRequest(request);
            Assert.AreEqual(500, data.StatusCode);
            Assert.AreEqual(0, data.Content.Length);
        }

        [TestMethod]
        public void HelloPlugin()
        {
            const string requestString = "GET /?action=Hello HTTP/1.1\n";
            var request = new Request(new MemoryStream(Encoding.Default.GetBytes(requestString)));

            var pm = PluginManager.getInstance();
            pm.PluginList.Add(new HelloPlugin());

            var data = pm.HandleRequest(request);
            Assert.AreEqual(200, data.StatusCode);
            Assert.AreEqual("Hello", Encoding.Default.GetString(data.Content));
            Assert.AreEqual("text/html", data.Contenttype);
            Assert.AreEqual(Data.DocumentTypeType.EmbeddedHtml, data.DocumentType);
            Assert.IsTrue(String.IsNullOrEmpty(data.Disposition));
        }
    }

    class BrokenPlugin : IPlugin
    {

        public string Name
        {
            get { return "BrokenPlugin"; }
        }

        public string Author
        {
            get { return "Mathias Gartner"; }
        }

        public Data CreateProduct(Request request)
        {
            throw new Exception("this plugin does not work");
        }
    }

    class HelloPlugin : IPlugin
    {
        public string Name
        {
	        get { return "Hello"; }
        }

        public string Author
        {
	        get { return "Mathias Gartner"; }
        }

        public Data CreateProduct(Request request)
        {
 	        var data = new Data();
            data.SetContent("Hello");
            return data;
        }
    }
}
