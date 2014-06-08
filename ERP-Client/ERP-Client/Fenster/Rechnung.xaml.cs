using System.Windows;
using ERPClient.ViewModels.FensterModels;

namespace ERPClient.Fenster
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
