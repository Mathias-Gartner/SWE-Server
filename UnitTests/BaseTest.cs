using Interface;
using System.IO;
using System.Text;

namespace UnitTests
{
    public class BaseTest
    {
        public Request RequestFromString(string request)
        {
            return new Request(new MemoryStream(Encoding.Default.GetBytes(request)));
        }

    }
}
