using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin.Data;
using ErpPlugin.Data.Fake;
using ErpPlugin;

namespace ErpUnitTests.Dal
{
    [TestClass]
    public class FakeDalTests
    {
        IDal fakeDal;

        [TestInitialize]
        public void Init()
        {
            CurrentDalFactory.Instance = new DalFactory();
            fakeDal = CurrentDalFactory.Instance.CreateDal();
        }

        [TestMethod]
        public void SearchContacts()
        {
            var contact = Contact.CreateSearchObject();
            contact.Lastname = "Huber";
            var contacts = fakeDal.Search(contact);
            Assert.AreEqual(2, contacts.Count);
            Assert.IsTrue(contacts.Any(c => c.Lastname == "Huber" && c.Firstname == "Hans"));
            Assert.IsTrue(contacts.Any(c => c.Lastname == "Huber" && c.Firstname == "Peter"));
        }
    }
}
