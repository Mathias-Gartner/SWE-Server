using System.Windows;
using ERPClient.ViewModels.FensterModels;

namespace ERPClient.Fenster
{
    public partial class NewRechnung : Window
    {
        public NewRechnung()
        {
            InitializeComponent();

            this.DataContext = new RechnungViewModel();
        }
    }
}
