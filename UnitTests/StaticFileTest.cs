using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interface;
using System.IO;
using System.Text;
using System.Reflection;

namespace UnitTests
{
    [TestClass]
    public class StaticFileTest : BaseTest
    {
        StaticFilePlugin.StaticFilePlugin plugin;
        string dir;

        [TestInitialize]
        public void Init()
        {
            plugin = new StaticFilePlugin.StaticFilePlugin();
            var filepath = Assembly.GetAssembly(this.GetType()).Location;
            dir = Path.GetDirectoryName(filepath);
            plugin.Path = dir;
        }

        [TestMethod]
        public void NotExistingFile()
        {
            var request = RequestFromString("GET /ghost HTTP/1.1\n");
            var data = plugin.CreateProduct(request);
            Assert.AreEqual(404, data.StatusCode);
            Assert.AreEqual(0, data.Content.Length);
        }

        [TestMethod]
        public void File()
        {
            System.IO.File.WriteAllText(dir + "\\testfile", "filetest");
            var request = RequestFromString("GET /testfile HTTP/1.1\n");
            var data = plugin.CreateProduct(request);
            Assert.AreEqual(200, data.StatusCode);
            Assert.AreEqual("filetest", Encoding.Default.GetString(data.Content));
            Assert.AreEqual(Data.DocumentTypeType.StandaloneFile, data.DocumentType);
            Assert.AreEqual("application/octet-stream", data.Contenttype);
            Assert.IsTrue(String.IsNullOrEmpty(data.Disposition));
        }

        [TestMethod]
        public void Directory()
        {
            System.IO.Directory.CreateDirectory(dir + "\\testdir");
            var request = RequestFromString("GET /testdir HTTP/1.1\n");
            var data = plugin.CreateProduct(request);
            Assert.AreEqual(200, data.StatusCode);
            Assert.IsTrue(data.Contenttype.Length > 0);
            Assert.AreEqual(Data.DocumentTypeType.EmbeddedHtml, data.DocumentType);
            Assert.AreEqual("text/html", data.Contenttype);
            Assert.IsTrue(String.IsNullOrEmpty(data.Disposition));
        }

        [TestMethod]
        public void FileDownload()
        {
            System.IO.File.WriteAllText(dir + "\\downloadfile", "testdownload");
            var request = RequestFromString("GET /downloadfile?download=1 HTTP/1.1\n");
            var data = plugin.CreateProduct(request);
            Assert.AreEqual(200, data.StatusCode);
            Assert.AreEqual("testdownload", Encoding.Default.GetString(data.Content));
            Assert.AreEqual(Data.DocumentTypeType.StandaloneFile, data.DocumentType);
            Assert.AreEqual("application/octet-stream", data.Contenttype);
            Assert.AreEqual("downloadfile", data.Disposition);
        }

        [TestMethod]
        public void DirUpAttack()
        {
            const string requestString = "GET /../ HTTP/1.1\n";
            var request = new Request(new MemoryStream(Encoding.Default.GetBytes(requestString)));

            var data = plugin.CreateProduct(request);
            Assert.AreEqual(403, data.StatusCode);
            Assert.AreEqual(0, data.Content.Length);
        }
    }
}
