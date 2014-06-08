using System.Windows;
using ERPClient.ViewModels.FensterModels;


namespace ERPClient.Fenster
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
