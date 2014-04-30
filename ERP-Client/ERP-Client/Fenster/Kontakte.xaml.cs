using System.Windows;
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

        private void BNeu_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ERP_Client.Fenster.AddKontakt();
            dlg.ShowDialog();
        }        
    }
}
