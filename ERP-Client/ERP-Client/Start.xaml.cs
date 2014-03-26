using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP_Client
{
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();
        }

        private void BKontakte_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ERP_Client.Fenster.Kontakte();
            dlg.ShowDialog();
        }

        private void NotImplement(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dieses Feature wurde noch nicht implementiert!", "Noch nicht implementiert");
        }

        
    }
}
