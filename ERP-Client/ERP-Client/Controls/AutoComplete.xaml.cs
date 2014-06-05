using ERP_Client.ViewModels;
using System;
using System.Collections;
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

namespace ERP_Client.Controls
{
    /// <summary>
    /// Interaction logic for AutoComplete.xaml
    /// </summary>
    public partial class AutoComplete : UserControl
    {
        #region Control based on MSDN example http://code.msdn.microsoft.com/windowsdesktop/WPF-Autocomplete-Textbox-df2f1791/sourcecode?fileId=48301&pathId=1237367252
        #region Members
        private System.Timers.Timer keypressTimer;
        private delegate void TextChangedCallback();
        private bool insertText;
        private int delayTime;
        private int searchThreshold;
        #endregion

        #region Constructor
        public AutoComplete()
        {
            InitializeComponent();

            setIcon.DataContext = this;
            unsetIcon.DataContext = this;
            textBox.DataContext = this;

            searchButton.DataContext = this;

            searchThreshold = 2;        // default threshold to 2 char

            // set up the key press timer
            keypressTimer = new System.Timers.Timer();
            keypressTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);

            // set up the text box and the combo box
            comboBox.IsSynchronizedWithCurrentItem = true;
            comboBox.IsTabStop = false;
            comboBox.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectionChanged);

            textBox.TextChanged += new TextChangedEventHandler(textBox_TextChanged);
            textBox.VerticalContentAlignment = VerticalAlignment.Center;
        }
        #endregion

        #region Methods
        public string Text
        {
            get { return textBox.Text; }
            set
            {
                insertText = true;
                textBox.Text = value;
            }
        }

        public int DelayTime
        {
            get { return delayTime; }
            set { delayTime = value; }
        }

        public int Threshold
        {
            get { return searchThreshold; }
            set { searchThreshold = value; }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != comboBox.SelectedItem)
            {
                insertText = true;
                ComboBoxItem cbItem = (ComboBoxItem)comboBox.SelectedItem;
                textBox.Text = cbItem.Content.ToString();
                IsSet = true;
                SelectedItem = cbItem.Tag;
            }
        }

        private void TextChanged()
        {
            try
            {
                IsSet = false;
                comboBox.Items.Clear();
                SelectedItem = null;
                if (textBox.Text.Length >= searchThreshold)
                {
                    foreach (object o in AutoCompleteSource.GetItems(textBox.Text))
                        comboBox.Items.Add(new ComboBoxItem() { Content = o.ToString(), Tag = o });

                    comboBox.IsDropDownOpen = comboBox.HasItems;

                    if (comboBox.Items.Count == 1 && ((string)comboBox.Items.OfType<ComboBoxItem>().Single().Content).ToLower() == textBox.Text.ToLower())
                    {
                        var cursor = textBox.SelectionStart;
                        var content = comboBox.Items.OfType<ComboBoxItem>().Single().Tag;
                        textBox.Text = content.ToString();
                        textBox.SelectionStart = cursor;
                        IsSet = true;
                        SelectedItem = content;
                    }
                }
                else
                {
                    comboBox.IsDropDownOpen = false;
                }
            }
            catch { }
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            keypressTimer.Stop();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                new TextChangedCallback(this.TextChanged));
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // text was not typed, do nothing and consume the flag
            if (insertText == true) insertText = false;

            // if the delay time is set, delay handling of text changed
            else
            {
                if (delayTime > 0)
                {
                    keypressTimer.Interval = delayTime;
                    keypressTimer.Start();
                }
                else TextChanged();
            }
        }

        #endregion
        #endregion

        private ICommandViewModel _search;
        public ICommandViewModel Search
        {
            get
            {
                if (_search == null)
                {
                    _search = new ExecuteCommandViewModel(
                        "Suchen",
                        "Suche in einem neuen Fenster ausführen",
                        () =>
                        {
                            var result = AutoCompleteSource.SearchInWindow(textBox.Text);
                            if (!String.IsNullOrEmpty(result))
                                textBox.Text = result;
                        });
                }
                return _search;
            }
        }

        public bool IsSet
        {
            get { return (bool)GetValue(IsSetProperty); }
            protected set { SetValue(IsSetProperty, value); }
        }

        protected static readonly DependencyProperty IsSetProperty =
            DependencyProperty.Register("IsSet", typeof(bool), typeof(AutoComplete));

        public IAutoCompleteSource AutoCompleteSource
        {
            get { return (IAutoCompleteSource)GetValue(AutoCompleteSourceProperty); }
            set { SetValue(AutoCompleteSourceProperty, value); }
        }

        public static readonly DependencyProperty AutoCompleteSourceProperty =
            DependencyProperty.Register("AutoCompleteSource", typeof(IAutoCompleteSource), typeof(AutoComplete));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set
            {
                if (IsSet)
                    SetValue(SelectedItemProperty, value);
                else
                    SetValue(SelectedItemProperty, null);
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(AutoComplete), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedItem_Changed));

        private static void SelectedItem_Changed(DependencyObject autoComplete, DependencyPropertyChangedEventArgs e)
        {
            var control = (AutoComplete)autoComplete;
            if (e.NewValue != null)
                control.textBox.Text = e.NewValue.ToString();
        }
    }

    public interface IAutoCompleteSource
    {
        IEnumerable GetItems(string searchExpression);

        string SearchInWindow(string searchString);
    }
}
