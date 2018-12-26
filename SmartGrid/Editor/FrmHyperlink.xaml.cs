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
using System.Windows.Shapes;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

namespace SmartGrid.Editor
{
    /// <summary>
    /// Interaction logic for FrmHyperlink.xaml
    /// </summary>
    public partial class FrmHyperlink : Window
    {
        public HyperlinkCreater Creater { get; }
        public FrmHyperlink(HyperlinkCreater creater) : base()
        {
            InitializeComponent();
            Creater = creater;
            if (string.IsNullOrEmpty(Creater.Text))
                Creater.Text = "\u25CF";
            DataContext = Creater;
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnFile_OnClick(object sender, RoutedEventArgs e)
        {
            var fd = new OpenFileDialog();
            if (fd.ShowDialog(this) == true)
                Creater.Uri = fd.FileName;
        }

        private void BtnFolder_OnClick(object sender, RoutedEventArgs e)
        {
            // Display a CommonOpenFileDialog to select only folders 
            CommonOpenFileDialog cfd = new CommonOpenFileDialog();
            cfd.EnsureReadOnly = true;
            cfd.IsFolderPicker = true;
            cfd.AllowNonFileSystemItems = true;
            if (cfd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ShellContainer selectedSO = null;
                try
                {
                    selectedSO = cfd.FileAsShellObject as ShellContainer;
                }
                catch
                {
                    MessageBox.Show("Could not create a ShellObject from the selected item");
                }
                Creater.Uri = selectedSO.ParsingName;
                this.Activate();
            }
        }
    }
}
