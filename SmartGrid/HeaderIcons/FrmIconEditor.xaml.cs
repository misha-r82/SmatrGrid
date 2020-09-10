using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace SmartGrid.HeaderIcons
{
    /// <summary>
    /// Логика взаимодействия для IconEditor.xaml
    /// </summary>

    public static class IconEditorCommands
    {
        static RoutedUICommand _addCommand = new RoutedUICommand("Добавить изображение", "AddIcon",typeof(IconEditorCommands));
        public static RoutedUICommand AddIcon
        {
            get => _addCommand;
        }
    }
    public partial class FrmIconEditor : Window
    {
        
        /*
        public static RoutedCommand Add { get; set; }

        static FrmIconEditor()
        {
                Add = new RoutedCommand("Add", typeof(FrmIconEditor));
        }*/
        public FrmIconEditor()
        {

            DataContext = WorkSpace.Instance.CoreHeaderIcon;
            InitializeComponent();
        }



        private void BtnRemove_OnClick(object sender, RoutedEventArgs e)
        {/*
            var selected = lstMain.SelectedItems.OfType<HeaderIcon>().ToArray();
            foreach (HeaderIcon icon in selected)
            {
                WorkSpace.Instance.CoreHeaderIcon.IconCollection.Remove(icon);
            }
            WorkSpace.Instance.CoreHeaderIcon.IconCollection.Remove(new HeaderIcon() { Name = "Новая иконка" });*/
        }


        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
        }
    }
}
