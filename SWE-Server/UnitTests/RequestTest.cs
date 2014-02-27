using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interface;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class RequestTest : BaseTest
    {
        [TestMethod]
        public void LeaveStreamOpen()
        {
            var stream = new MemoryStream();
            Assert.IsTrue(stream.CanRead);
            Assert.IsTrue(stream.CanWrite);
            var request = new Request(stream);
            Assert.IsTrue(stream.CanRead);
            Assert.IsTrue(stream.CanWrite);
        }

        [TestMethod]
        public void UrlWithParameters()
        {
            var request = RequestFromString("GET /dir/file.ext?param=value&param2=abc HTTP/1.1\n");
            Assert.AreEqual(Url.MethodType.GET, request.Url.Method);
            Assert.AreEqual("/dir/file.ext", request.Url.Path);
            Assert.AreEqual(2, request.Url.Parameters.Count);
            Assert.AreEqual("value", request.Url.Parameters["param"]);
            Assert.AreEqual("abc", request.Url.Parameters["param2"]);
        }

        [TestMethod]
        public void NoUrlPostRequest()
        {
            var request = RequestFromString("GET  HTTP/1.1\n");
            Assert.AreEqual(Url.MethodType.GET, request.Url.Method);
            Assert.AreEqual("", request.Url.Path);
        }

        [TestMethod]
        public void GetParameterWithoutValue()
        {
            var request = RequestFromString("GET /?download\n");
            Assert.AreEqual(Url.MethodType.GET, request.Url.Method);
            Assert.AreEqual("/", request.Url.Path);
            Assert.AreEqual(1, request.Url.Parameters.Count);
            Assert.IsTrue(request.Url.Parameters.ContainsKey("download"));
        }

        [TestMethod]
        public void PostParameterWithoutValue()
        {
            var requestString = String.Join(Environment.NewLine, new[] {
                "POST / HTTP/1.1",
                "Content-Type: application/x-www-form-urlencoded",
                "Content-Length: 11",
                "",
                "param=value"
            });
            var request = RequestFromString(requestString);
            Assert.AreEqual(2, request.Header.Count);
            Assert.AreEqual("application/x-www-form-urlencoded", request.Header["content-type"]);
            Assert.AreEqual("11", request.Header["content-length"]);
            Assert.AreEqual(1, request.PostData.Count);
            Assert.AreEqual("value", request.PostData["param"]);
            Assert.AreEqual("/", request.Url.Path);
            Assert.AreEqual(Url.MethodType.POST, request.Url.Method);
        }

        [TestMethod]
        public void IfModifiedSinceHeader()
        {
            const string requestString = "GET /cachefile HTTP/1.1\nIf-Modified-Since: Sun, 01 Jan 2090 GMT";
            var request = RequestFromString(requestString);
            Assert.AreEqual(1, request.Header.Count);
            Assert.AreEqual("sun, 01 jan 2090 gmt", request.Header["if-modified-since"]);
            Assert.AreEqual("/cachefile", request.Url.Path);
            Assert.AreEqual(Url.MethodType.GET, request.Url.Method);
        }
    }
}
