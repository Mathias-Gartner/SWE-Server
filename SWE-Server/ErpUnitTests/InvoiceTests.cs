using ErpPlugin;
using ErpPlugin.Data;
using ErpPlugin.Data.Fake;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpUnitTests
{
    [TestClass]
    public class InvoiceTests : ErpBaseTest
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
            var invoice = new Invoice() { Message = "test" };
            invoice.Save();
            Assert.AreEqual(BusinessObjectState.Unmodified, invoice.State);
            Assert.IsTrue(ErpPlugin.Data.Fake.Dal.InvoiceSaved);
        }

        [TestMethod]
        public void GenerateInvoiceDocumentTest()
        {
            var invoice = new Invoice();
            invoice.Comment = "Not relevant.";
            invoice.Contact = new Contact { Firstname = "John", Lastname = "Doe" };
            invoice.DueDate = DateTime.Parse("2014-01-01");
            invoice.InvoiceDate = invoice.DueDate;
            invoice.InvoiceNumber = 12;
            invoice.Message = "Kitchen machines";
            invoice.Outgoing = true;
            invoice.Entries.Add(new InvoiceEntry() { Amount = 1, Description = "Microwave oven", Price = 39.95m, UStPercent = 20 });
            invoice.Entries.Add(new InvoiceEntry() { Amount = 1, Description = "Steamer", Price = 299, UStPercent = 20 });

            var pdf = invoice.GenerateInvoiceDocument();
            Assert.IsTrue(pdf.Length > 10000);

            //var text = Encoding.UTF8.GetString(pdf);
            /*Assert.IsFalse(text.Contains("Not relevant."));
            Assert.IsTrue(text.Contains("John Doe"));
            Assert.IsTrue(text.Contains("Kitchen machines"));
            Assert.IsTrue(text.Contains("Nr. 12"));
            Assert.IsTrue(text.Contains("Microwave oven"));
            Assert.IsTrue(text.Contains("Steamer"));
            Assert.IsTrue(text.Contains("Summe: " + invoice.Sum.ToString("#,##0.00")));*/
        }

    }
}
