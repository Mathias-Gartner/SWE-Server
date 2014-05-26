using System.Windows;
using ERP_Client.ViewModels.FensterModels;

namespace ERP_Client.Fenster
{
    public partial class Rechnung : Window
    {
        public Rechnung()
        {
            InitializeComponent();

            this.DataContext = new RechnungViewModel();
        }
    }
}
