using System.Windows;
using ERP_Client.ViewModels.FensterModels;

namespace ERP_Client.Fenster
{
    /// <summary>
    /// Interaktionslogik für AddKontakt.xaml
    /// </summary>
    public partial class AddKontakt : Window
    {
        public AddKontakt()
        {
            InitializeComponent();

            this.DataContext = new KontaktViewModel();
        }
    }
}
