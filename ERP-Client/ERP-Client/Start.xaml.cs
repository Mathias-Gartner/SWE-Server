using System.Windows;
using ERP_Client.ViewModels.FensterModels;

namespace ERP_Client
{
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();

            this.DataContext = new KontaktViewModel();
        }

        private void BKontakte_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ERP_Client.Fenster.Kontakte();
            dlg.ShowDialog();
        }

        private void NotImplement(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dieses Feature wurde noch nicht implementiert!", "Noch nicht implementiert");
        }        
    }
}
