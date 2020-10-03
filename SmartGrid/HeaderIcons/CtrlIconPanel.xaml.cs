﻿using System;
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

namespace SmartGrid.HeaderIcons
{
    /// <summary>
    /// Логика взаимодействия для CtrlIconPanel.xaml
    /// </summary>
    public partial class CtrlIconPanel : UserControl
    {
        public CtrlIconPanel()
        {
            InitializeComponent();
        }
        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var icon = e.Parameter as HeaderIcon;
            WorkSpace.Instance.CurElement.Header.Icons.Add(icon);
        }
    }
}