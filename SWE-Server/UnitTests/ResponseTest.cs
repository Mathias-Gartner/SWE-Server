using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interface;
using SWE_Server;
using System.IO;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class ResponseTest : BaseTest
    {
        [TestMethod]
        public void ResponseCode304()
        {
            ResponseCodeTest(304, "HTTP/1.1 304 Not Modified", "");
        }

        [TestMethod]
        public void ResponseCode400()
        {
            ResponseCodeTest(400, "HTTP/1.1 400 Bad Request", "<h1>400 Bad Request</h1>");
        }

        [TestMethod]
        public void ResponseCode403()
        {
            ResponseCodeTest(403, "HTTP/1.1 403 Forbidden", "<h1>403 Forbidden</h1>");
        }

        [TestMethod]
        public void ResponseCode404()
        {
            ResponseCodeTest(404, "HTTP/1.1 404 Not Found", "<h1>404 Url not found</h1>");
        }

        [TestMethod]
        public void ResponseCode500()
        {
            ResponseCodeTest(500, "HTTP/1.1 500 Internal Server Error", "<h1>500 Internal Server Error</h1>");
        }

        private void ResponseCodeTest(int code, string header, string body)
        {
            var stream = new MemoryStream();
            var data = new Data();
            data.StatusCode = code;

            var response = new Response(data);
            response.send(stream);

            var text = Encoding.Default.GetString(stream.ToArray());
            Assert.IsTrue(text.StartsWith(header));
            Assert.IsTrue(text.ToLower().Contains("content-length: " + body.Length));
            Assert.IsTrue(text.EndsWith(body));
        }
    }
}
