using System.Windows;
using ERPClient.ViewModels.FensterModels;

namespace ERPClient.Fenster
{
    public partial class SearchFirma : Window
    {
        public SearchFirma()
        {
            InitializeComponent();

            this.DataContext = new KontaktViewModel(() => this.Close(), true, null);
        }
    }
}
