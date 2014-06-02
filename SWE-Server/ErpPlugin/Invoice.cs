using ErpPlugin.Data;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin
{
    public class Invoice : BusinessObject
    {
        public bool Outgoing { get; set; }

        public int InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string Message { get; set; }

        public string Comment { get; set; }

        public Contact Contact { get; set; }

        public IList<InvoiceEntry> Entries { get; set; }

        public ICollection<Invoice> Search()
        {
            if (State != BusinessObjectState.SearchObject)
                throw new InvalidOperationException("Only SearchObjects can be used for searching");

            try
            {
                return CurrentDalFactory.Instance.CreateDal().SearchInvoice(this);
            }
            catch (ObjectNotFoundException)
            {
                return new List<Invoice>();
            }
        }

        public bool Save()
        {
            return CurrentDalFactory.Instance.CreateDal().SaveInvoice(this);
        }

        public static Invoice CreateSearchObject()
        {
            return CreateSearchObject<Invoice>();
        }

        public byte[] GenerateInvoiceDocument()
        {
            var doc = new Document();
            var section = doc.AddSection();

            // TODO: Make headline configureable
            var p = section.AddParagraph("Beispielfirma");
            p.Format.Font.Size = 16;
            p.Format.Font.Bold = true;

            section.AddParagraph("Beispielstraße 1");
            section.AddParagraph("1010 Wien");
            section.AddParagraph();
            section.AddParagraph();
            if (String.IsNullOrEmpty(Contact.Name))
                section.AddParagraph(String.Join(" ", new[] { Contact.Firstname, Contact.Lastname }));
            else
                section.AddParagraph(Contact.Name);

            if (Contact.InvoiceAddress == null)
            {
                section.AddParagraph();
                section.AddParagraph();
            }
            else
            {
                if (!(String.IsNullOrEmpty(Contact.InvoiceAddress.Street) && String.IsNullOrEmpty(Contact.InvoiceAddress.Number)))
                    section.AddParagraph(String.Join(" ", new[] { Contact.InvoiceAddress.Street, Contact.InvoiceAddress.Number }));
                section.AddParagraph(String.Join(" ", new[] { Contact.InvoiceAddress.PostalCode, Contact.InvoiceAddress.City }));
            }
            section.AddParagraph();
            section.AddParagraph();

            p = section.AddParagraph("Rechnung");
            if (InvoiceNumber > 0)
                p.AddText(String.Format(" Nr. {0}", InvoiceNumber));

            var table = section.AddTable();
            var headRow = table.AddRow();
            headRow.Format.Font.Bold = true;
            
            AddCell(headRow, "Menge");
            AddCell(headRow, "Artikel");
            AddCell(headRow, "Netto");
            AddCell(headRow, "USt");
            AddCell(headRow, "Brutto");
            
            decimal sum = 0;
            foreach(var entry in Entries)
            {
                var row = table.AddRow();
                AddCell(row, entry.Amount.ToString("#,##0"));
                AddCell(row, entry.Description);
                AddCell(row, (entry.Amount * entry.NetPrice).ToString("#,##0.00"));
                AddCell(row, entry.UStPercent.ToString("00"));
                AddCell(row, (entry.Amount * entry.Price).ToString("#,##0.00"));
                sum += entry.Price;
            }

            p = section.AddParagraph("Summe: " + sum.ToString("#,##0.00"));
            p.Format.Alignment = ParagraphAlignment.Right;
            
            var renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = doc;
            renderer.RenderDocument();
            var stream = new MemoryStream();
            renderer.PdfDocument.Save(stream);
            return stream.ToArray();
        }

        private void AddCell(Row row, string content)
        {
            var cell = new Cell();
            cell.AddParagraph(content);
            row.Cells.Add(cell);
        }
    }
}
