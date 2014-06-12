using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ERPClient.Fenster;
using ERPClient.ViewModels.FensterModels;

namespace ERPClient.ViewModels
{
    public class SingleRechnungViewModel : ViewModel
    {
        public SingleRechnungViewModel(RechnungViewModel parent, Invoice i)
        {
            Parent = parent;
            Invoice = i;
        }

        public RechnungViewModel Parent { get; protected set; }

        public bool SearchMode { get { return Parent.SearchMode; } }

        public Invoice Invoice { get; set; }

        public string RNummer
        {
            get
            {
                return Invoice.InvoiceNumber.ToString();
            }
        }

        public string RDatum
        {
            get
            {
                return Invoice.InvoiceDate.ToString();
            }
        }

        private ICommand _editRechnung;
        public ICommand EditRechnung
        {
            get
            {
                if (_editRechnung == null)
                    _editRechnung = new EditRechnungCommand(this, Invoice);
                return _editRechnung;
            }
        }        

        private abstract class SingleRechnungCommand : ICommand
        {
            protected Invoice invoce;
            protected SingleRechnungViewModel _singleRechnungViewModel;

            protected SingleRechnungCommand(SingleRechnungViewModel singleRechnungViewModel, Invoice Invoice)
            {
                this._singleRechnungViewModel = singleRechnungViewModel;
                this.invoce = Invoice;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public abstract void Execute(object parameter);
        }

        private sealed class EditRechnungCommand : SingleRechnungCommand
        {
            public EditRechnungCommand(SingleRechnungViewModel singleRechnungViewModel, Invoice invoice)
                : base(singleRechnungViewModel, invoice)
            { }

            public override void Execute(object parameter)
            {
                NewRechnung neueRechnung = new NewRechnung(invoce);
                neueRechnung.ShowDialog();
                _singleRechnungViewModel.Parent.SucheRechnung();
            }
        }

        private ICommandViewModel _PdfDownload;
        public ICommandViewModel PdfDownload
        {
            get
            {
                if (_PdfDownload == null)
                {
                    _PdfDownload = new ExecuteCommandViewModel(
                        "PDF Dowload",
                        "Rechnung als PDF dowloaden",
                        DownloadPdf);
                }
                return _PdfDownload;
            }
        }

        public void DownloadPdf()
        {
            Proxy proxy = new Proxy();

            proxy.RechnungPdf(Invoice);
        }
    }
}
