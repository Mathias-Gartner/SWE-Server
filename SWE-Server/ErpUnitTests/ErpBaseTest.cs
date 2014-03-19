using Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests;

namespace ErpUnitTests
{
    public class ErpBaseTest : BaseTest
    {
        public Request XmlRequestFromString(string xml)
        {
            var request = string.Format("POST /?action=erp HTTP/1.1\nContent-Type: text/xml\nContent-Length: {0}\n\n{1}", xml.Length, xml);
            return new Request(new MemoryStream(Encoding.Default.GetBytes(request)));
        }
    }
}
