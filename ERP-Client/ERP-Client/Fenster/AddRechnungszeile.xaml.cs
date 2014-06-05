using System.Windows;
using ERP_Client.ViewModels.FensterModels;

namespace ERP_Client.Fenster
{
    public partial class AddRechnungszeile : Window
    {
        public AddRechnungszeile()
        {
            InitializeComponent();

            this.DataContext = new RechnungViewModel();
        }
    }
}
