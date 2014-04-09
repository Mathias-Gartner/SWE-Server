using System;
using System.Windows.Input;

namespace ERP_Client.ViewModels
{
    public interface ICommandViewModel : ICommand
    {
        string Label { get; set; }
        string ToolTip { get; set; }
        void OnCanExecuteChanged();
    }
    
    public class CommandViewModel : ViewModel, ICommandViewModel
    {
        public CommandViewModel(string label, string toolTip)
        {
            _label = label;
            _toolTip = toolTip;
        }

        private string _label;

        public string Label
        {
            get { return _label; }
            set { _label = value; OnPropertyChanged("Label"); }
        }
        private string _toolTip;

        public string ToolTip
        {
            get { return _toolTip; }
            set { _toolTip = value; OnPropertyChanged("ToolTip"); }
        }
        
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void OnCanExecuteChanged()
        {
            var temp = CanExecuteChanged;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }

        public virtual void Execute(object parameter)
        {

        }
    }

    public class ExecuteCommandViewModel : CommandViewModel
    {
        public ExecuteCommandViewModel(string label, string toolTip, Action execute)
            : this(label, toolTip, execute, null)
        {
        }

        public ExecuteCommandViewModel(string label, string toolTip, Action execute, Func<bool> canExecute = null)
            : base(label, toolTip)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            this._execute = execute;
            this._canExecute = canExecute;
        }

        private Action _execute;
        private Func<bool> _canExecute;

        public override bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }
            return base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _execute();
        }
    }

    public class ExecuteParameterCommandViewModel<TParam> : CommandViewModel
    {
        public ExecuteParameterCommandViewModel(string label, string toolTip, Action<TParam> execute)
            : this(label, toolTip, execute, null)
        {
        }

        public ExecuteParameterCommandViewModel(string label, string toolTip, Action<TParam> execute, Func<bool> canExecute = null)
            : base(label, toolTip)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            this._execute = execute;
            this._canExecute = canExecute;
        }

        private Action<TParam> _execute;
        private Func<bool> _canExecute;

        public override bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }
            return base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            if (parameter is TParam) 
            { 
                _execute((TParam)parameter);
            }
            else
            {
                _execute(default(TParam));
            }
        }
    }
}
