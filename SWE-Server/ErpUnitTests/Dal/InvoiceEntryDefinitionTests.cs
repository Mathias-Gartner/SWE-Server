using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin;
using ErpPlugin.Data.Definitions;

namespace ErpUnitTests.Dal
{
    [TestClass]
    public class InvoiceEntryDefinitionTests
    {
        [TestMethod]
        public void CreateArguments()
        {
            var ie = new InvoiceEntry();
            ie.Amount = 142;
            ie.Description = "Büroklammern 100 Pack";
            ie.Price = 2.99m;
            ie.UStPercent = 20;

            var dal = new InvoiceEntryDefinition();
            var a = dal.CreateArguments(ie);

            Assert.AreEqual(142, a["amount"]);
            Assert.AreEqual("Büroklammern 100 Pack", a["description"]);
            Assert.AreEqual(2.99m, a["price"]);
            Assert.AreEqual(20, a["ustPercent"]);
        }
    }
}
