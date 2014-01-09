using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interface;


namespace ServerzeitPlugin
{
    public class ServerzeitPlugin:IPlugin
    {
        public string Name
        {
            get { return "Serverzeit"; }
        }

        public string Author
        {
            get { return "Christoph Posch"; }
        }

        public struct SystemTime
        {
            public int Year;
            public int Month;
            public int Day;
            public int Hour;
            public int Minute;
            public int Second;
        };

        public Data CreateProduct(Request request)
        {
            SystemTime time = new SystemTime();
            DateTime jetzt = DateTime.Now;

            time.Year = jetzt.Year;
            time.Month = jetzt.Month;
            time.Day = jetzt.Day;
            time.Hour = jetzt.Hour;
            time.Minute = jetzt.Minute;
            time.Second = jetzt.Second;

            ASCIIEncoding encoding = new ASCIIEncoding();

            string buffer = "<div>Aktuelle Serverzeit:<br />" + time.Day.ToString() + "." + time.Month.ToString() + "." + time.Year.ToString() + "<br />" + time.Hour.ToString() + ":";
            
            if(time.Minute < 10)
                buffer += "0" + time.Minute.ToString() + ":";   //sieht statt 12:0:15 so aus: 12:00:15
            else
                buffer += time.Minute.ToString() + ":";
            
            if(time.Second < 10)
                buffer += "0" + time.Second.ToString() + "</div>";      //sieht statt 12:15:5 so aus: 12:15:05
            else
                buffer += time.Second.ToString() + "</div>";

            byte[] Ausgabe = encoding.GetBytes(buffer);
            
            Data data = new Data();
            data.Content = Ausgabe;

            return data;

        }
    }
}
