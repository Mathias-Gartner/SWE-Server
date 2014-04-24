using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin.Data.Database;
using ErpPlugin;
using ErpPlugin.Data.Definitions;

namespace ErpUnitTests.Dal
{
    [TestClass]
    public class IDefinitionFactoryTests
    {
        [TestMethod]
        public void ContactDefinition()
        {
            var factory = new DefinitionFactory();
            var definition = factory.CreateDefinitionForType<Contact>();
            Assert.AreEqual(typeof(ContactDefinition), definition.GetType());
        }

        [TestMethod]
        public void UserDefinition()
        {
            var factory = new DefinitionFactory();
            var definition = factory.CreateDefinitionForType<User>();
            Assert.AreEqual(typeof(UserDefinition), definition.GetType());
        }

        [TestMethod]
        public void AddressDefinition()
        {
            var factory = new DefinitionFactory();
            var definition = factory.CreateDefinitionForType<Address>();
            Assert.AreEqual(typeof(AddressDefinition), definition.GetType());
        }
    }
}
