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

namespace SmartGrid.Controls
{
    /// <summary>
    /// Логика взаимодействия для CtrlFontCommands.xaml
    /// </summary>
    public partial class CtrlFontCommands : UserControl
    {
        public CtrlFontCommands()
        {
            InitializeComponent();
        }

        private void CommandBold_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var element in WorkSpace.Instance.Curent.SelectedElements)
                element.Header.Style.Bold = !element.Header.Style.Bold;

        }

        private void CommandItalic_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var element in WorkSpace.Instance.Curent.SelectedElements)
                element.Header.Style.Italic = !element.Header.Style.Italic;
        }

        private void CommandUndeline_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var element in WorkSpace.Instance.Curent.SelectedElements)
                element.Header.Style.Underline = !element.Header.Style.Underline;
        }
    }
}
