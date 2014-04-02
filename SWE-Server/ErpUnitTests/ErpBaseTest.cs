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
        public Request XmlRequestFromString(string requestType, string xml)
        {
            xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + xml;
            var request = string.Format("POST /?action=erp&req={0} HTTP/1.1\nContent-Type: text/xml\nContent-Length: {1}\n\n{2}", requestType, xml.Length, xml);
            return new Request(new MemoryStream(Encoding.Default.GetBytes(request)));
        }
    }
}
