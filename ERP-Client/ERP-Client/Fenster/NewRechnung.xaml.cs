using System.Windows;
using ERP_Client.ViewModels.FensterModels;

namespace ERP_Client.Fenster
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
