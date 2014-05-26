using System.Windows;
using ERP_Client.ViewModels.FensterModels;

namespace ERP_Client
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
