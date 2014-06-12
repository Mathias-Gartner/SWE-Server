using ErpPlugin.Data;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpPlugin
{
    public class Invoice : BusinessObject
    {
        public Invoice()
        {
            Entries = new Collection<InvoiceEntry>();
        }

        public bool? Outgoing { get; set; }

        public int InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? InvoiceDateFrom { get; set; }

        public DateTime? InvoiceDateTo { get; set; }

        public DateTime? DueDate { get; set; }

        public string Message { get; set; }

        public string Comment { get; set; }

        public Contact Contact { get; set; }

        public Collection<InvoiceEntry> Entries { get; set; }

        public decimal Sum { get { return Entries == null ? 0 : Entries.Sum(e => e.Amount * e.Price); } }

        public decimal? SumFrom { get; set; }

        public decimal? SumTo { get; set; }

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
            if (State != BusinessObjectState.New)
                return false;

            return CurrentDalFactory.Instance.CreateDal().Save(this);
        }

        public static Invoice CreateSearchObject()
        {
            var searchObject = CreateSearchObject<Invoice>();
            searchObject.InvoiceNumber = -1;
            return searchObject;
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
            p.Format.Font.Bold = true;
            if (InvoiceNumber > 0)
                p.AddText(String.Format(" Nr. {0}", InvoiceNumber));

            section.AddParagraph();

            var table = section.AddTable();
            for (var i = 0; i < 6; i++)
                table.AddColumn();
            
            var headRow = table.AddRow();
            headRow.Format.Font.Bold = true;
            
            AddCell(headRow, "Menge");
            AddCell(headRow, "Artikel");
            AddCell(headRow, "Netto");
            AddCell(headRow, "USt");
            AddCell(headRow, "Brutto");
            AddCell(headRow, "Summe");
            
            decimal sum = 0;
            foreach(var entry in Entries)
            {
                var row = table.AddRow();
                AddCell(row, entry.Amount.ToString("#,##0"));
                AddCell(row, entry.Description);
                AddCell(row, entry.NetPrice.ToString("#,##0.00"));
                AddCell(row, entry.UStPercent.ToString("00"));
                AddCell(row, entry.Price.ToString("#,##0.00"));
                AddCell(row, (entry.Price * entry.Amount).ToString("#,##0.00"));
                sum += entry.Price;
            }

            section.AddParagraph();
            p = section.AddParagraph();
            p.AddFormattedText("Summe: ", TextFormat.Bold);
            p.AddText(sum.ToString("#,##0.00"));
            p.Format.Alignment = ParagraphAlignment.Right;
            
            var renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = doc;
            renderer.RenderDocument();
            var stream = new MemoryStream();
            renderer.PdfDocument.Save(stream);
            return stream.ToArray();
        }

        private static void AddCell(Row row, string content)
        {
            var cell = new Cell();
            cell.AddParagraph(content);
            row.Cells.Add(cell);
        }
    }
}
