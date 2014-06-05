using System.Windows;
using ERP_Client.ViewModels.FensterModels;


namespace ERP_Client.Fenster
{
    public partial class Kontakte : Window
    {
        private KontaktViewModel kontaktViewModel;

        public Kontakte() : this(false, null)
        { }

        public Kontakte(bool searchMode, string searchString)
        {
            InitializeComponent();

            kontaktViewModel = new KontaktViewModel(() => this.Close(), searchMode, searchString);
            this.DataContext = kontaktViewModel;
        }

        public string Result
        {
            get { return kontaktViewModel.Result; }
        }
    }
}
