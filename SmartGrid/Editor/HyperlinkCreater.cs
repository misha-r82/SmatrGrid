using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using SmartGrid.Annotations;

namespace SmartGrid.Editor
{
    public class HyperlinkCreater : INotifyPropertyChanged
    {
        private string _uri;
        public string Text { get; set; }

        public string Uri
        {
            get { return _uri; }
            set
            {
                if (_uri == value) return;
                _uri = value;
                OnPropertyChanged();
            }
        }

        public string ToolTip { get; set; }

        public HyperlinkCreater(string text)
        {
            Text = text;
            Uri = "";
            ToolTip = "";
        }
        public HyperlinkCreater(Hyperlink link)
        {
            if (link != null)
            {
                var tr = new TextRange(link.ContentStart, link.ContentEnd);
                Text = tr.Text;
                Uri = link.NavigateUri.ToString();
                ToolTip = link.ToolTip.ToString();
            }
        }
        public Hyperlink Link
        {
            get
            {
                Hyperlink hyperLink = new Hyperlink();
                hyperLink.IsEnabled = true;

                hyperLink.Inlines.Add(Text);
                try
                {
                    hyperLink.NavigateUri = new Uri(Uri);
                }
                catch (Exception e)
                {

                }

                hyperLink.ToolTip = ToolTip;
                return hyperLink;
            }       
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
