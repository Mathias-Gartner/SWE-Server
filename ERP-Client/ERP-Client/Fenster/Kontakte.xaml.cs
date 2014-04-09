using System.Collections.Generic;
using System.Windows;
using ERP_Client;
using ERP_Client.ViewModels.FensterModels;


namespace ERP_Client.Fenster
{
    public partial class Kontakte : Window
    {
        public Kontakte()
        {
            InitializeComponent();

            this.DataContext = new KontaktViewModel();
        }
    }
}
