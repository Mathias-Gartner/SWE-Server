using ErpPlugin;
using ErpPlugin.Data;
using ErpPlugin.Data.Fake;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpUnitTests
{
    [TestClass]
    public class ContactTests : ErpBaseTest
    {
        [TestInitialize]
        public void Init()
        {
            CurrentDalFactory.Instance = new DalFactory();
            ErpPlugin.Data.Fake.Dal.Reset();
        }

        [TestMethod]
        public void Save()
        {
            var contact = new Contact() { Firstname = "John", Lastname = "Doe" };
            contact.Save();
            Assert.AreEqual(BusinessObjectState.Unmodified, contact.State);
            Assert.IsTrue(ErpPlugin.Data.Fake.Dal.ContactSaved);
        }
    }
}
