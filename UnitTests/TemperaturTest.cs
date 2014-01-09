using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TemperaturTest:BaseTest
    {
        TemperaturPlugin.TemperaturPlugin plugin;

        [TestInitialize]
        public void Init()
        {
            plugin = new TemperaturPlugin.TemperaturPlugin();
        }

        [TestMethod]
        public void TemperatureSearchHtml()
        {
            var buffer = "<form action=\"/\" method=\"get\">";
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
            
            string bufferTest1 = "01.01.2003 00:00:00&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-10&degC<br />01.01.2003 08:00:00&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-9,999671&degC<br />01.01.2003 16:00:00&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-9,998683&degC<br />";
            string bufferTest2 = "Kein Treffer<br />M&ouml;gliche Ursache: kein g&uuml;tiges Datum";
            string bufferTest3 = "29.02.2004 00:00:00&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-0,588678&degC<br />29.02.2004 08:00:00&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-0,491413&degC<br />29.02.2004 16:00:00&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-0,393805&degC<br />";
                
            var request = RequestFromString("GET /?action=Temperatur&Suche_jahr=2003&Suche_monat=1&Suche_tag=1 HTTP/1.1");
            var data = plugin.CreateProduct(request);
            Assert.AreEqual(buffer + bufferTest1, Encoding.Default.GetString(data.Content));

            request = RequestFromString("GET /?action=Temperatur&Suche_jahr=2003&Suche_monat=2&Suche_tag=29 HTTP/1.1");
            data = plugin.CreateProduct(request);
            Assert.AreEqual(buffer + bufferTest2, Encoding.Default.GetString(data.Content));

            request = RequestFromString("GET /?action=Temperatur&Suche_jahr=2004&Suche_monat=2&Suche_tag=29 HTTP/1.1");
            data = plugin.CreateProduct(request);
            Assert.AreEqual(buffer + bufferTest3, Encoding.Default.GetString(data.Content));

            request = RequestFromString("GET /?action=Temperatur&Suche_jahr=2012&Suche_monat=11&Suche_tag=31 HTTP/1.1");
            data = plugin.CreateProduct(request);
            Assert.AreEqual(buffer + bufferTest2, Encoding.Default.GetString(data.Content));
        }

        [TestMethod]
        public void TemperatureSearchXml()
        {
            string bufferTest0 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Suchergebnis><Text>Dies ist ein XML-Format</Text></Suchergebnis>";
            string bufferTest1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Suchergebnis><Text>Dies ist ein XML-Format</Text><Ergebnis><Datum>01.01.2003 00:00:00</Datum><Messwert>-10</Messwert></Ergebnis><Ergebnis><Datum>01.01.2003 08:00:00</Datum><Messwert>-9,999671</Messwert></Ergebnis><Ergebnis><Datum>01.01.2003 16:00:00</Datum><Messwert>-9,998683</Messwert></Ergebnis></Suchergebnis>";
            string bufferTest2 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Suchergebnis><Text>Dies ist ein XML-Format</Text><Ergebnis><Datum>29.02.2004 00:00:00</Datum><Messwert>-0,588678</Messwert></Ergebnis><Ergebnis><Datum>29.02.2004 08:00:00</Datum><Messwert>-0,491413</Messwert></Ergebnis><Ergebnis><Datum>29.02.2004 16:00:00</Datum><Messwert>-0,393805</Messwert></Ergebnis></Suchergebnis>";

            var request = RequestFromString("GET /?action=Temperatur&Suche_jahr=2003&Suche_monat=1&Suche_tag=1&Format=xml HTTP/1.1");
            var data = plugin.CreateProduct(request);
            Assert.AreEqual(bufferTest1, Encoding.Default.GetString(data.Content));

            request = RequestFromString("GET /?action=Temperatur&Suche_jahr=2003&Suche_monat=2&Suche_tag=29&Format=xml HTTP/1.1");
            data = plugin.CreateProduct(request);
            Assert.AreEqual(bufferTest0, Encoding.Default.GetString(data.Content));

            request = RequestFromString("GET /?action=Temperatur&Suche_jahr=2004&Suche_monat=2&Suche_tag=29&Format=xml HTTP/1.1");
            data = plugin.CreateProduct(request);
            Assert.AreEqual(bufferTest2, Encoding.Default.GetString(data.Content));

            request = RequestFromString("GET /?action=Temperatur&Suche_jahr=2012&Suche_monat=11&Suche_tag=31&Format=xml HTTP/1.1");
            data = plugin.CreateProduct(request);
            Assert.AreEqual(bufferTest0, Encoding.Default.GetString(data.Content));
        }
    }
}
