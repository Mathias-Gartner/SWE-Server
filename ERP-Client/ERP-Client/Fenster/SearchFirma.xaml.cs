using System.Windows;
using ERP_Client.ViewModels.FensterModels;

namespace ERP_Client.Fenster
{
    public partial class SearchFirma : Window
    {
        public SearchFirma()
        {
            InitializeComponent();

            this.DataContext = new KontaktViewModel();
        }
    }
}
