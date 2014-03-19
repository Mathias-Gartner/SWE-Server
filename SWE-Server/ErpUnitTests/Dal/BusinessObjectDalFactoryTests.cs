using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin.Data.Database;
using ErpPlugin;

namespace ErpUnitTests.Dal
{
    [TestClass]
    public class BusinessObjectDalFactoryTests
    {
        [TestMethod]
        public void ContactDal()
        {
            var factory = new BusinessObjectDalFactory();
            var dal = factory.CreateDalForType<Contact>();
            Assert.AreEqual(typeof(ContactDal), dal.GetType());
        }

        [TestMethod]
        public void UserDal()
        {
            var factory = new BusinessObjectDalFactory();
            var dal = factory.CreateDalForType<User>();
            Assert.AreEqual(typeof(UserDal), dal.GetType());
        }
    }
}
