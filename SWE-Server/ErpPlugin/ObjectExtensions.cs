using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ErpPlugin.Xml
{
    public static class ObjectExtensions
    {
        public static string ToXmlString(this object obj)
        {
            if (obj == null) { throw new ArgumentNullException("obj"); }

            XmlSerializer xml = new XmlSerializer(obj.GetType());
            StringBuilder sb = new StringBuilder();
            xml.Serialize(XmlWriter.Create(sb), obj);
            return sb.ToString();
        }

        public static T FromXmlString<T>(this char[] xml) where T : class
        {
            var stringreader = new StringReader(new string(xml));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            return xmlSerializer.Deserialize(stringreader) as T;
        }
    }
}
