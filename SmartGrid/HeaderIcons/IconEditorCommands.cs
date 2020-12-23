using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;

namespace SmartGrid.HeaderIcons
{
    /// <summary>
    /// Логика взаимодействия для IconEditor.xaml
    /// </summary>
    public static class HeaderIconCommands
    {
        private static readonly AddIconToHeaderCommand _addIconToHeaderCommand = new AddIconToHeaderCommand();
        public static AddIconToHeaderCommand AddIconToHederCommand
        {
            get => _addIconToHeaderCommand;
        }
        public class AddIconToHeaderCommand:ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                var icon = parameter as IconElement;
                foreach (IHasHeader header in WorkSpace.Instance.Curent.SelectedElements)
                    header.Header.Icons.Add(icon);
            }
            public event EventHandler CanExecuteChanged;
        }

        public class SetIconCommand : ICommand
        {
            private IconElement _icon;
            private IconSet _iconSet;
            public SetIconCommand(IconElement icon, IconSet iconSet)
            {
                _icon = icon;
                _iconSet = iconSet;
            }
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                _iconSet.Add(_icon);
            }
            public event EventHandler CanExecuteChanged;
        }
    }
    public static class IconEditorCommands
    {
        private static readonly AddCollectionCommand _addCollectionCommand = new AddCollectionCommand();
        private static readonly AddIconCommand _addIconCommand = new AddIconCommand();

        public static AddIconCommand AddIconComamnd => _addIconCommand;
        public static AddCollectionCommand AddCollection => _addCollectionCommand;

        internal static IEnumerable<string> ShowFileDialog()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            if (fileDialog.ShowDialog() == true) return fileDialog.FileNames;
            return null;
        }
        public class AddCollectionCommand : ICommand
        {
            public bool CanExecute(object parameter)
            {
                return _collection != null;
            }
            private IconCollection _collection;
            public IconCollection Collection
            {
                get => _collection;
                set
                {
                    if (_collection == value) return;
                    _collection = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            public void Execute(object parameter)
            {
                var filenames = ShowFileDialog();
                if (filenames == null || !filenames.Any()) return;
                foreach (var fileName in filenames)
                {
                    if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) continue;
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    var stream = new FileStream(fileName, FileMode.Open);
                    var coll = Collection.Group.CreateCollection(name);
                    coll.FirstIcon.FromStream(stream);
                }
            }
            public event EventHandler CanExecuteChanged;
        }
        public class AddIconCommand : ICommand
        {
            private IconElement _icon;
            public IconElement SelectedIcon
            {
                get => _icon;
                set
                {
                    if (_icon == value) return;
                    _icon = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            private IconCollection _collection;
            public IconCollection Collection
            {
                get => _collection;
                set
                {
                    if (_collection == value) return;
                    _collection = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            public bool CanExecute(object parameter)
            {
                return _icon != null || _collection!= null;
            }
            public void Execute(object parameter)
            {
                var filenames = IconEditorCommands.ShowFileDialog();
                if (filenames == null || !filenames.Any()) return;
                foreach (var fileName in filenames)
                {
                    if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) continue;
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    var stream = new FileStream(fileName, FileMode.Open);
                    var collection = _icon != null ? _icon.Collection : _collection;
                    collection.CreatElement(name, stream);
                }
            }
            public event EventHandler CanExecuteChanged;
        }

    }
}