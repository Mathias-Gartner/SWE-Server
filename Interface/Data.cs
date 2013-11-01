using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Data
    {
        public Data()
        {
            Simpletype = false;
            Contenttype = "text/html";
            Disposition = null;
        }

        public string Disposition { get; set; } // Nur setzen falls Download
        public byte[] Content { get; set; }     // Content (was Plugin zurückgibt)
        public bool Simpletype { get; set; }    // Simpletype true wenn content ein komplettes File ist
        public string Contenttype { get; set; } // Contenttype des Content, normal text/html, ansonsten anpassen
    }
}
