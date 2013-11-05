using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Server
{
    class ExceptionHandler
    {
        public static void ErrorMsg(int ErrorNr, Exception e = null)
        {
            switch (ErrorNr)
            {
                case 1: Console.Write("Directory nicht gefunden"); break;
                case 2: Console.Write("File nicht gefunden"); break;
                case 3: Console.Write("Error sending response"); break;
                case 4: Console.Write("Plugin failed"); break;
            }

            if (e != null)
                Console.Write(": " + e.Message);

            Console.WriteLine();
        }
    }
}
