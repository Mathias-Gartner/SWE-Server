using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Interface;

namespace TemperaturPlugin
{
    public class TemperaturPlugin : IPlugin
    {
        public string Name
        {
            get { return "Temperatur"; }
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
            public int DayOfYear;
        };

        bool erstens = false;

        public Data CreateProduct(Request request)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            if (erstens)
            {
                TemperaturMesser();
                erstens = true;
            }

            //using(connection = new SqlConnection("Data Source=CHRISTOPH-VAIO;Initial Catalog=TemperaturMessung;Integrated Security=True"))
            try
            {
                connection = new SqlConnection("Data Source=.;Initial Catalog=TemperaturMessung;Integrated Security=True");
                connection.Open();

                Data Ausgabe = new Data();
                ASCIIEncoding encoding = new ASCIIEncoding();
                string buffer = null;

                string format = null;
                request.Url.Parameters.TryGetValue("Format", out format);

                if(format != "xml")
                {
                    buffer = "<form action=\"/\" method=\"get\">";
                    buffer += "<input type=\"hidden\" name=\"action\" value=\"Temperatur\">";
                    buffer += "<select name=\"Suche_tag\">";
                    buffer += "<option value=01>1</option><option value=02>2</option><option value=03>3</option><option value=04>4</option><option value=05>5</option><option value=06>6</option><option value=07>7</option><option value=08>8</option><option value=09>9</option><option value=10>10</option>"
                    + "<option value=11>11</option><option value=12>12</option><option value=13>13</option><option value=14>14</option><option value=15>15</option><option value=16>16</option><option value=17>17</option><option value=18>18</option><option value=19>19</option><option value=20>20</option>"
                    + "<option value=21>21</option><option value=22>22</option><option value=23>23</option><option value=24>24</option><option value=25>25</option><option value=26>26</option><option value=27>27</option><option value=28>28</option><option value=29>29</option><option value=30>30</option><option value=31>31</option>";
                    buffer += "</select>";
                    buffer += "<select name=\"Suche_monat\">";
                    buffer += "<option value=01>1</option><option value=02>2</option><option value=03>3</option><option value=04>4</option><option value=05>5</option><option value=06>6</option><option value=07>7</option><option value=08>8</option><option value=09>9</option><option value=10>10</option><option value=11>11</option><option value=12>12</option>";
                    buffer += "</select>";
                    buffer += "<select name=\"Suche_jahr\" >";
                    buffer += "<option value=2003>2003</option><option value=2004>2004</option><option value=2005>2005</option><option value=2006>2006</option><option value=2007>2007</option><option value=2008>2008</option><option value=2009>2009</option><option value=2010>2010</option><option value=2011>2011</option><option value=2012>2012</option><option value=2013>2013</option>";
                    buffer += "</select>";
                    buffer += "&nbsp;<input type=\"submit\" value=\"Suchen\">";
                    buffer += "</form><br /><br />";
                }

                string query = null;
                string seite = null;
                string tag = null, monat = null, jahr = null;
                bool seite_ja = false;
                bool suche_ja = false;
                int x = 1, i = 0;

                suche_ja = request.Url.Parameters.TryGetValue("Suche_tag", out tag);
                suche_ja = request.Url.Parameters.TryGetValue("Suche_monat", out monat);
                suche_ja = request.Url.Parameters.TryGetValue("Suche_jahr", out jahr);                

                if (suche_ja)
                {
                    query = "SELECT Zeit, Messwert FROM Temperatur "
                        + "WHERE Zeit = @datum0 "
                        + "OR Zeit = @datum8 "
                        + "OR Zeit = @datum16";

                    SqlCommand cmd = new SqlCommand(query, connection);

                    try
                    {
                        DateTime zeit = DateTime.Parse(jahr + "-" + monat + "-" + tag + " 00:00:00");
                        cmd.Parameters.AddWithValue("@datum0", zeit);

                        zeit = DateTime.Parse(jahr + "-" + monat + "-" + tag + " 08:00:00");
                        cmd.Parameters.AddWithValue("@datum8", zeit);

                        zeit = DateTime.Parse(jahr + "-" + monat + "-" + tag + " 16:00:00");
                        cmd.Parameters.AddWithValue("@datum16", zeit);

                        reader = cmd.ExecuteReader();
                    }
                    catch { }

                    if (reader == null)
                    {
                        if (format == "xml")
                        {
                            Ausgabe.DocumentType = Data.DocumentTypeType.StandaloneFile;
                            Ausgabe.Contenttype = "text/xml";

                            buffer += "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Suchergebnis><Text>Dies ist ein XML-Format</Text></Suchergebnis>";
                        }
                        else
                            buffer += "Kein Treffer<br />M&ouml;gliche Ursache: kein g&uuml;tiges Datum";
                    }
                    else
                    {                        
                        DataTable datatable = new DataTable();
                        datatable.Load(reader);

                        if (format == "xml")
                        {
                            Ausgabe.DocumentType = Data.DocumentTypeType.StandaloneFile;
                            Ausgabe.Contenttype = "text/xml";

                            //Ausgabe Text
                            buffer += "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Suchergebnis><Text>Dies ist ein XML-Format</Text>";

                            foreach (DataRow row in datatable.Rows)
                            {
                                buffer += "<Ergebnis><Datum>" + datatable.Rows[i]["Zeit"].ToString() + "</Datum><Messwert>" + datatable.Rows[i]["Messwert"].ToString() + "</Messwert></Ergebnis>";                                
                                i++;
                            }

                            buffer += "</Suchergebnis>";
                        }
                        else
                        {
                            foreach (DataRow row in datatable.Rows)
                            {
                                buffer += datatable.Rows[i]["Zeit"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + datatable.Rows[i]["Messwert"].ToString() + "&degC<br />";
                                i++;
                            }
                        }
                    }
                }

                else
                {
                    query = "SELECT Zeit, Messwert FROM Temperatur";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    reader = cmd.ExecuteReader();

                    DataTable datatable = new DataTable();
                    datatable.Load(reader);
                    int gesamtZeilen = datatable.Rows.Count;

                    seite_ja = request.Url.Parameters.TryGetValue("page", out seite);

                    if (seite_ja)
                        x = Convert.ToInt32(seite);

                    i = 30 * (x - 1);

                    while (i < 30 * x)
                    {
                        buffer += datatable.Rows[i]["Zeit"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + datatable.Rows[i]["Messwert"].ToString() + "&degC<br />";
                        i++;

                        if (i == gesamtZeilen)
                            break;
                    }

                    string vorige_Seite = Convert.ToString(x - 1);
                    string naechste_Seite = Convert.ToString(x + 1);

                    if (x > 2)
                        buffer += "<a href=/?action=Temperatur&page=" + vorige_Seite + ">Vorherige Seite</a>&nbsp;&nbsp;&nbsp;";
                    if (x == 2)
                        buffer += "<a href=/?action=Temperatur>Vorherige Seite</a>&nbsp;&nbsp;&nbsp;";

                    if(i != gesamtZeilen)
                        buffer += "<a href=/?action=Temperatur&page=" + naechste_Seite + ">N&auml;chste Seite</a>";
                }
                
                byte[] Buffer = encoding.GetBytes(buffer);
               
                Ausgabe.Content = Buffer;

                return Ausgabe;
            }

            finally
            {
                if (connection != null)
                    connection.Close();

                if (reader != null)
                    reader.Close();
            }
        }

        void TemperaturMesser()
        {
            Thread thread = new Thread(new ThreadStart(DoMessen));
            thread.Start();
        }

        void DoMessen()
        {
            SqlConnection connection2 = null;
            SqlDataReader reader2 = null;
            SystemTime time = new SystemTime();
            DateTime jetzt;
            SqlCommand cmd;
            string query2;
            int d, x;
            float wert;
            
            while (true)
            {
                jetzt = DateTime.Now;
                
                time.Hour = jetzt.Hour;

                if (time.Hour == 0 || time.Hour == 8 || time.Hour == 16)
                {
                    time.Minute = jetzt.Minute;
                    time.Second = jetzt.Second;
                    if (time.Minute == 0)
                        if (time.Second == 0)
                        {
                            time.Year = jetzt.Year;
                            time.Month = jetzt.Month;
                            time.Day = jetzt.Day;
                            time.DayOfYear = jetzt.DayOfYear;

                            try
                            {
                                connection2 = new SqlConnection("Data Source=CHRISTOPH-VAIO;Initial Catalog=TemperaturMessung;Integrated Security=True");
                                connection2.Open();

                                if (time.Year == 2012 || time.Year == 2016 || time.Year == 2020 || time.Year == 2024)
                                    d = 1098;
                                else
                                    d = 1095;

                                if (time.Hour == 0)
                                    x = 3;
                                else if (time.Hour == 8)
                                    x = 2;
                                else
                                    x = 1;

                                wert = (float)(-1 * Math.Cos((2 * Math.PI / d) * (time.DayOfYear * 3 - x)) * 20) + 10;

                                if (time.Hour < 10)
                                    query2 = "INSERT INTO Temperatur(Messwert, Zeit) VALUES(" + wert.ToString() + ", '" + time.Year.ToString() + time.Month + time.Day.ToString() + " 0" + time.Hour.ToString() + ":00:00')";
                                else
                                    query2 = "INSERT INTO Temperatur(Messwert, Zeit) VALUES(" + wert.ToString() + ", '" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + " " + time.Hour.ToString() + ":00:00')";

                                //Beispiel: query2 = "INSERT INTO Temperatur(Messwert, Zeit) VALUES(-10.000000, '20030101 00:00:00')";

                                cmd = new SqlCommand(query2, connection2);
                                reader2 = cmd.ExecuteReader();

                            }

                            finally
                            {
                                if (connection2 != null)
                                    connection2.Close();

                                if (reader2 != null)
                                    reader2.Close();
                            }
                        }
                }

                Thread.Sleep(1000);
            }
        }
    }
}
