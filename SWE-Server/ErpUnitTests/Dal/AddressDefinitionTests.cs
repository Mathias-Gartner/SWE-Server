using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin;
using ErpPlugin.Data.Definitions;

namespace ErpUnitTests.Dal
{
    [TestClass]
    public class AddressDefinitionTests
    {
        [TestMethod]
        public void CreateArguments()
        {
            var address = new Address();
            address.City = "Wien";
            address.Country = "Austria";
            address.Name = "FH Technikum Wien";
            address.Number = "1";
            address.PostalCode = "1200";
            address.PostOfficeBox = "-";
            address.Street = "Hochstädlplatz";

            var dal = new AddressDefinition();
            var a = dal.CreateArguments(address);

            Assert.AreEqual("Wien", a["city"]);
            Assert.AreEqual("Austria", a["country"]);
            Assert.AreEqual("FH Technikum Wien", a["name"]);
            Assert.AreEqual("1", a["number"]);
            Assert.AreEqual("1200", a["postalCode"]);
            Assert.AreEqual("-", a["postOfficeBox"]);
            Assert.AreEqual("Hochstädlplatz", a["street"]);
        }
    }
}
