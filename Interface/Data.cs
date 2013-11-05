using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Data
    {
        [DefaultValue(EmbeddedHtml)]
        public enum DocumentTypeType
        {
            EmbeddedHtml,
            StandaloneFile
        }

        public Data()
        {
            Content = new byte[0];
            Contenttype = "text/html";
            Disposition = null;
            StatusCode = 200;
        }

        public void SetContent(string text)
        {
            Content = Encoding.Default.GetBytes(text);
        }

        public string Disposition { get; set; } // Nur setzen falls Download
        public byte[] Content { get; set; }     // Content (was Plugin zurückgibt)
        public DocumentTypeType DocumentType { get; set; }    // Legt fest ob der Content in das HTML Template eingebettet wird
        public string Contenttype { get; set; } // Contenttype des Content, normal text/html, ansonsten anpassen
        public int StatusCode { get; set; }
    }
}
