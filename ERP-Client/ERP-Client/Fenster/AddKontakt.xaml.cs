using System.Windows;
using ERPClient.ViewModels.FensterModels;

namespace ERPClient.Fenster
{
    /// <summary>
    /// Interaktionslogik für AddKontakt.xaml
    /// </summary>
    public partial class AddKontakt : Window
    {
        public AddKontakt()
        {
            InitializeComponent();

            this.DataContext = new AddKontaktViewModel();
        }

        public AddKontakt(Contact c)
        {
            InitializeComponent();

            this.DataContext = new AddKontaktViewModel(c);
        }
    }
}
