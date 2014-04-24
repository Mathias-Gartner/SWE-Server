using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin;
using ErpPlugin.Data.Database;
using ErpPlugin.Data.Definitions;

namespace ErpUnitTests.Dal
{
    [TestClass]
    public class ContactDefinitionTests : ErpBaseTest
    {
        [TestMethod]
        public void CreateArguments()
        {
            var contact = new Contact();
            contact.DateOfBirth = DateTime.Parse("2014-10-11");
            contact.Email = "huber@localhost.com";
            contact.Firstname = "Hans";
            contact.Lastname = "Huber";
            contact.Name = "Huber KG";
            contact.Prefix = "Dr.";
            contact.Suffix = "MSc";
            contact.Uid = "U12";

            var dal = new ContactDefinition();
            var a = dal.CreateArguments(contact);

            Assert.AreEqual(DateTime.Parse("2014-10-11"), a["dateOfBirth"]);
            Assert.AreEqual("huber@localhost.com", a["email"]);
            Assert.AreEqual("Hans", a["firstname"]);
            Assert.AreEqual("Huber", a["lastname"]);
            Assert.AreEqual("Huber KG", a["name"]);
            Assert.AreEqual("Dr.", a["prefix"]);
            Assert.AreEqual("MSc", a["suffix"]);
            Assert.AreEqual("U12", a["uid"]);
        }
    }
}
