using System.Collections.Generic;
using System.Windows;
using ERP_Client;
using ERP_Client.ViewModels.FensterModels;


namespace ERP_Client.Fenster
{
    public partial class Kontakte : Window
    {
        public Kontakte()
        {
            InitializeComponent();

            this.DataContext = new KontaktViewModel();
        }

        private void KontaktSuche(object sender, RoutedEventArgs e)
        {
            Proxy proxy = new Proxy();
            Contact contact = new Contact();
            Contact kontakt = new Contact();
            List<Contact> liste = new List<Contact>();
            int anzahl = 0;
            string text;
                        
            //TODO: Use CommandViewModel

            contact.State = "SearchObject";
            contact.Firstname = ((KontaktViewModel)DataContext).Firstname;
            contact.Lastname = ((KontaktViewModel)DataContext).Lastname;

            liste = proxy.KontaktSuchen(contact);

            text = "Suchergebnis:";

            anzahl = liste.Count;

            if (anzahl > 0)
            {
                for (int i = 0; i < anzahl; i++)
                {
                    kontakt = liste[i];

                    if (kontakt.Name != null || kontakt.Uid != null)
                        text += "\n" + kontakt.Name + " " + kontakt.Uid + " ";
                    if (kontakt.Firstname != null || kontakt.Lastname != null)
                        text += "\n" + kontakt.Firstname + " " + kontakt.Lastname + " ";

                    //text += kontakt.Address.Street + " " + kontakt.Address.Number + " " + kontakt.Address.PostalCode + " " + kontakt.Address.City;
                }
            }
            else
                text += "\nKeine Kontakte gefunden.";

            Suchergebnis.Text = text;
        }
    }
}
