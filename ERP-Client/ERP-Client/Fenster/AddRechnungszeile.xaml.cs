using System.Windows;
using ERPClient.ViewModels.FensterModels;

namespace ERPClient.Fenster
{
    public partial class AddRechnungszeile : Window
    {
        public AddRechnungszeile(RechnungViewModel rvm)
        {
            InitializeComponent();

            this.DataContext = rvm;
        }
    }
}
