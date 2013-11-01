using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Server
{
    class ExceptionHandler
    {
        public static void ErrorMsg(int ErrorNr)
        {
            switch (ErrorNr)
            {
                case 1: Console.WriteLine("Directory nicht gefunden"); break;
                case 2: Console.WriteLine("File nicht gefunden"); break;
            }
        }
    }
}
