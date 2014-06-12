using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ErpPlugin;
using ErpPlugin.Data.Definitions;

namespace ErpUnitTests.Dal
{
    [TestClass]
    public class InvoiceDefinitionTests
    {
        [TestMethod]
        public void CreateArguments()
        {
            var invoice = new Invoice();
            invoice.DueDate = DateTime.Parse("2014-10-11");
            invoice.InvoiceDate = DateTime.Parse("2014-10-11");
            invoice.Comment = "comment";
            invoice.Message = "message";
            invoice.Outgoing = true;
            invoice.InvoiceNumber = 11;

            var dal = new InvoiceDefinition();
            var a = dal.CreateArguments(invoice);

            Assert.AreEqual(DateTime.Parse("2014-10-11"), a["dueDate"]);
            Assert.AreEqual(DateTime.Parse("2014-10-11"), a["invoiceDate"]);
            Assert.AreEqual("comment", a["comment"]);
            Assert.AreEqual("message", a["message"]);
            Assert.AreEqual(true, a["outgoingInvoice"]);
            Assert.AreEqual(11, a["invoiceNumber"]);
        }
    }
}
