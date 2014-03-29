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
            //TODO: Use CommandViewModel

            Contact c = new Contact();
            c.State = "SearchObject";
            c.Firstname = ((KontaktViewModel)DataContext).Firstname;
            c.Lastname = ((KontaktViewModel)DataContext).Lastname;

            Proxy p = new Proxy();
            p.KontaktSuchen(c);
        }
    }
}
