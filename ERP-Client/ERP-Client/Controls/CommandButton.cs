using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ERPClient.ViewModels;

namespace ERPClient.Controls
{
    public class CommandButton : Button
    {
        public CommandButton()
        {
            var self = RelativeSource.Self;

            var bCmd = new Binding("CommandViewModel");
            bCmd.RelativeSource = self;
            this.SetBinding(CommandProperty, bCmd);

            var bLabel = new Binding("CommandViewModel.Label");
            bLabel.RelativeSource = self;
            this.SetBinding(ContentProperty, bLabel);

            var bTooltip = new Binding("CommandViewModel.ToolTip");
            bTooltip.RelativeSource = self;
            this.SetBinding(ToolTipProperty, bTooltip);

            this.SetValue(ToolTipService.ShowOnDisabledProperty, true);

            this.Loaded += new RoutedEventHandler(CommandButton_Loaded);
        }

        void CommandButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                this.Content = "Command Button";
            }
            this.CommandTarget = this;
        }

        public ICommandViewModel CommandViewModel
        {
            get { return (ICommandViewModel)GetValue(CommandViewModelProperty); }
            set { SetValue(CommandViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandViewModelProperty =
            DependencyProperty.Register("CommandViewModel", typeof(ICommandViewModel), typeof(CommandButton));
    }
}
