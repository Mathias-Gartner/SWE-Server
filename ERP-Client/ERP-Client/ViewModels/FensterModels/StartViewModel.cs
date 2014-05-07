using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Client.ViewModels.FensterModels
{
    public class StartViewModel : ViewModel
    {
        #region Button
        private ICommandViewModel _Kontakte;
        public ICommandViewModel Kontakte
        {
            get
            {
                if (_Kontakte == null)
                {
                    _Kontakte = new ExecuteCommandViewModel(
                        "Kontakte",
                        "Verwalten von Kontakten",
                        OpenKontakte);
                }
                return _Kontakte;
            }
        }

        #endregion

        public void OpenKontakte()
        {
            var dlg = new ERP_Client.Fenster.Kontakte();
            dlg.ShowDialog();
        }
    }
}
