using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin.Data;
using ErpPlugin.Data.Fake;

namespace ErpUnitTests
{
    [TestClass]
    public class FacadeTests : ErpBaseTest
    {
        //const string c_contactSearchRequest = "<request xmlns=\"http://stud12.technikum-wien.at/~if12b007/SweErpXmlSchema.xsd\"><authentication><username>User1</username><password>123456</password></authentication><search><Contact><State>New</State><ID>-1</ID><Firstname>Hans</Firstname><Lastname>Huber</Lastname><dateOfBirth>1960-01-01T00:00:00</dateOfBirth></Contact></search></request>";
        const string c_contactSearchRequest = "<Contact xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><State>SearchObject</State><Firstname>Hans</Firstname><Lastname>Huber</Lastname><DateOfBirth>1960-01-01T00:00:00</DateOfBirth></Contact>";

        ErpPlugin.ErpPlugin erpPlugin;

        [TestInitialize]
        public void Init()
        {
            CurrentDalFactory.Instance = new FakeDalFactory();
            erpPlugin = new ErpPlugin.ErpPlugin();
        }

        [TestMethod]
        public void GetRequest()
        {
            var request = RequestFromString("GET /?action=erp HTTP/1.1");
            var data = erpPlugin.CreateProduct(request);
            Assert.AreEqual(400, data.StatusCode); //Bad Request
            Assert.AreEqual("<h1>Postdata required</h1>", Encoding.Default.GetString(data.Content));
        }

        [TestMethod]
        public void InvalidXml()
        {
            var request = XmlRequestFromString("searchContact", "<don't look at me, I'm not valid xml");
            var data = erpPlugin.CreateProduct(request);
            Assert.AreEqual(400, data.StatusCode);
            //Assert.IsTrue(Encoding.Default.GetString(data.Content).StartsWith("<h1>Posted XML is invalid</h1>"));
            Assert.AreEqual("<h1>Error in XML</h1>", Encoding.Default.GetString(data.Content));
        }

        [TestMethod]
        public void SchemaViolation()
        {
            var request = XmlRequestFromString("searchContact", "<request xmlns=\"http://stud12.technikum-wien.at/~if12b007/SweErpXmlSchema.xsd\"><authentication></authentication><search></search></request>");
            var data = erpPlugin.CreateProduct(request);
            Assert.AreEqual(400, data.StatusCode);
            //Assert.IsTrue(Encoding.Default.GetString(data.Content).StartsWith("<h1>Postdata schema valiation failed</h1>"));
            Assert.AreEqual("<h1>Error in XML</h1>", Encoding.Default.GetString(data.Content));
        }

        [TestMethod]
        public void WrongSchema()
        {
            var request = XmlRequestFromString("searchContact", "<abc><def>ghi</def></abc>");
            var data = erpPlugin.CreateProduct(request);
            Assert.AreEqual(400, data.StatusCode);
            //Assert.IsTrue(Encoding.Default.GetString(data.Content).StartsWith("<h1>Postdata schema valiation failed</h1>"));
            Assert.AreEqual("<h1>Error in XML</h1>", Encoding.Default.GetString(data.Content));
        }

        [TestMethod]
        public void AuthenticateWrongPassword()
        {
            var request = XmlRequestFromString("searchContact", c_contactSearchRequest.Replace("123456", "wrongPw"));
            var data = erpPlugin.CreateProduct(request);
            Assert.AreEqual(401, data.StatusCode);
            Assert.AreEqual("<h1>Invalid credentials</h1>", Encoding.Default.GetString(data.Content));
        }

        [TestMethod]
        public void AuthenticateWrongUser()
        {
            var request = XmlRequestFromString("searchContact", c_contactSearchRequest.Replace("User1", "UserXX"));
            var data = erpPlugin.CreateProduct(request);
            Assert.AreEqual(401, data.StatusCode);
            Assert.AreEqual("<h1>Invalid credentials</h1>", Encoding.Default.GetString(data.Content));
        }

        [TestMethod]
        public void SearchContact()
        {
            var request = XmlRequestFromString("searchContact", c_contactSearchRequest);
            var data = erpPlugin.CreateProduct(request);
            var response = Encoding.Default.GetString(data.Content);
            //Assert.IsTrue(response.Contains("<status>success</status>"));
            Assert.IsTrue(response.Contains("<Contact"));
            Assert.IsTrue(response.Contains("<Firstname>Hans</Firstname>"));
            Assert.IsTrue(response.Contains("<Firstname>Peter</Firstname>"));
        }
    }
}
