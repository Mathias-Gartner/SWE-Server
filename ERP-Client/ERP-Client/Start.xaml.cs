using System.Windows;
using ERPClient.ViewModels.FensterModels;

namespace ERPClient
{
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();

            this.DataContext = new StartViewModel();
        }
    }
}
