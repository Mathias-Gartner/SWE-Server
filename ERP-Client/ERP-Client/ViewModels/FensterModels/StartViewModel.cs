using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERPClient.ViewModels.FensterModels
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

        private ICommandViewModel _Rechnungen;
        public ICommandViewModel Rechnungen
        {
            get
            {
                if (_Rechnungen == null)
                {
                    _Rechnungen = new ExecuteCommandViewModel(
                        "Rechnungen",
                        "Verwalten von Rechnungen",
                        OpenRechnung);
                }
                return _Rechnungen;
            }
        }

        private ICommandViewModel _Projekte;
        public ICommandViewModel Projekte
        {
            get
            {
                if (_Projekte == null)
                {
                    _Projekte = new ExecuteCommandViewModel(
                        "Projekte",
                        "Verwalten von Projekten",
                        NotImplement);
                }
                return _Projekte;
            }
        }

        private ICommandViewModel _Angebote;
        public ICommandViewModel Angebote
        {
            get
            {
                if (_Angebote == null)
                {
                    _Angebote = new ExecuteCommandViewModel(
                        "Angebote",
                        "Verwalten von Angeboten",
                        NotImplement);
                }
                return _Angebote;
            }
        }

        private ICommandViewModel _Zeiterfassung;
        public ICommandViewModel Zeiterfassung
        {
            get
            {
                if (_Zeiterfassung == null)
                {
                    _Zeiterfassung = new ExecuteCommandViewModel(
                        "Zeiterfassung",
                        "Zeiterfassung",
                        NotImplement);
                }
                return _Zeiterfassung;
            }
        }

        private ICommandViewModel _Angestellte;
        public ICommandViewModel Angestellte
        {
            get
            {
                if (_Angestellte == null)
                {
                    _Angestellte = new ExecuteCommandViewModel(
                        "Angestellte",
                        "Verwalten von Angestellten",
                        NotImplement);
                }
                return _Angestellte;
            }
        }

        private ICommandViewModel _Bankkonto;
        public ICommandViewModel Bankkonto
        {
            get
            {
                if (_Bankkonto == null)
                {
                    _Bankkonto = new ExecuteCommandViewModel(
                        "Bankkonto",
                        "Verwalten des Bankkontos",
                        NotImplement);
                }
                return _Bankkonto;
            }
        }

        private ICommandViewModel _Logon;
        public ICommandViewModel Logon
        {
            get
            {
                if (_Logon == null)
                {
                    _Logon = new ExecuteCommandViewModel(
                        "Login",
                        "Mit seinem Account anmelden",
                        NotImplement);
                }
                return _Logon;
            }
        }
        #endregion

        public static void OpenKontakte()
        {
            var dlg = new ERPClient.Fenster.Kontakte();
            dlg.ShowDialog();
        }

        public static void OpenRechnung()
        {
            var dlg = new ERPClient.Fenster.Rechnung();
            dlg.ShowDialog();
        }

        public static void NotImplement()
        {
            MessageBox.Show("Dieses Feature wurde noch nicht implementiert!", "Noch nicht implementiert");
        }
    }
}
