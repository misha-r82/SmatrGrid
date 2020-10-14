using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Lib;

namespace SmartGrid.Controls
{
    /// <summary>
    /// Логика взаимодействия для EditableLable.xaml
    /// </summary>
    public partial class EditableLable : UserControl
    {
        public EditableLable()
        {
            InitializeComponent();

        }

        static EditableLable()
        {
            TextProperty = DependencyProperty.Register(
                    "Text",
                    typeof(string),
                    typeof(EditableLable),
                    new FrameworkPropertyMetadata(
                        string.Empty,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        }


        public static readonly DependencyProperty TextProperty;
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        private bool _isEditing;
        public bool DoubleClick { get; set; } = true;
        public bool IsEditing
        {
            get
            {
                return _isEditing;
            }
            set
            {
                if (_isEditing == value) return;
                _isEditing = value;
                label.Visibility = !_isEditing ? Visibility.Visible : Visibility.Collapsed;
                textBox.Visibility = _isEditing ? Visibility.Visible : Visibility.Collapsed;
                if (_isEditing) Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input,
                            new Action(delegate () { Keyboard.Focus(textBox); }));
            }
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DoubleClick & e.ClickCount < 2) return;
            IsEditing = true;
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsEditing = false;
        }



        private void Label_OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.Key == Key.Enter || args.Key == Key.F2)
                IsEditing = true;
        }

        private UIElement GetFirstFocusable(UIElement element)
        {
            DependencyObject current = VisualTreeHelper.GetParent(element);
            while (current != null)
            {
                var ui = current as UIElement;
                if (ui != null && ui.Focusable) return ui;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
        private FrameworkElement _prewItem = null;

        private void TextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void EditableLable_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(label);
        }
    }
}
