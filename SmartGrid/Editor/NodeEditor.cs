using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace SmartGrid.Editor
{
    class NodeEditor
    {
        private static RichTextBox _rtb;
        private static Node _node;
        public static RichTextBox Rtb
        {
            get { return _rtb; }
            set
            {
                if (_rtb!= null && Equals(_rtb, value)) return;
                _rtb = value;
                _node = (Node)_rtb.DataContext;
                _rtb.IsDocumentEnabled = true;
                _rtb.Document.IsEnabled = true;
                _rtb.Document.PageWidth = _rtb.ActualWidth;
                _rtb.SetValue(Paragraph.LineHeightProperty, 1.0);
                var range = new TextRange(_rtb.Document.ContentStart, _rtb.Document.ContentEnd);
                try
                {
                    MemoryStream stream = new MemoryStream(_node.ValBin);
                    range.Load(stream, DataFormats.XamlPackage);
                    stream.Close();
                }
                catch (Exception e)
                {
                }

                foreach (var hyp in Hyperlinks)
                {
                    hyp.RequestNavigate += Hyperlink_RequestNavigate;
                }
                    
                Debug.WriteLine("set rtb");
            }
        }
        public static string SelectedText
        {
            get { return Rtb.Selection.Text; }
        }
        public static TextPointer Caret { get
        {
            return Rtb.CaretPosition;
        }}

        public static TextRange DocRng
        {
            get
            {
                return new TextRange(_rtb.Document.ContentStart, _rtb.Document.ContentEnd);
            }
        }
        public static void SaveToNodeVal()
        {
            try
            {
                //foreach (var hyp in Hyperlinks)
                //    hyp.RequestNavigate -= Hyperlink_RequestNavigate;
                MemoryStream stream = new MemoryStream();
                DocRng.Save(stream, DataFormats.XamlPackage);
                _node.ValBin = stream.ToArray();
                stream.Close();
            }
            catch (Exception ex)
            {
            }
        }

        private static IEnumerable<Hyperlink> Hyperlinks
        {
            get
            {
                foreach (Block block in _rtb.Document.Blocks)
                {
                    var p = block as Paragraph;
                    if (p == null) continue;
                    foreach (Inline inline in p.Inlines)
                    {
                        var hyp = inline as Hyperlink;
                        if (hyp != null) yield return hyp;
                    }
                }
            }
        }


        private static void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start("explorer.exe", e.Uri.OriginalString);
        }

        public static bool Overlaps(TextElement element, TextPointer start, TextPointer end)
        {
            return element.ContentEnd.CompareTo(start) > 0 && element.ContentStart.CompareTo(end) < 0;
        }

        public static Inline CaretElement
        {
            get
            {
                Inline inline = null;
                TextPointer curRng = Rtb.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
                var paragraph = Rtb.CaretPosition.Paragraph;
                if (paragraph == null) return null;
                foreach (Inline cur in paragraph.Inlines)
                {
                    if (Overlaps(cur, curRng, curRng))
                        inline = cur;
                }

                if (inline == null && paragraph.Inlines.Any())
                    return paragraph.Inlines.Last();
                return inline;
            }
        }

        public static void HyperlinkCmd()
        {
            var link = CaretElement as Hyperlink;
            var creater = link == null ? new HyperlinkCreater(SelectedText): new HyperlinkCreater(link);
            var f = new FrmHyperlink(creater);
            if (f.ShowDialog() == true)
            {
                if (link != null)
                {
                    link.NavigateUri = new Uri(f.Creater.Uri);
                    link.Inlines.Clear();
                    link.Inlines.Add(f.Creater.Text);
                    link.ToolTip = f.Creater.ToolTip;
                } else InsertHyperLink(f.Creater.Link);
            }
        }
        public static void InsertHyperLink(Hyperlink link)
        {
            link.RequestNavigate += Hyperlink_RequestNavigate;
            Rtb.Selection.Text = "";
            Paragraph paragraph = Caret.Paragraph;
            if (paragraph == null) return;
            var inline = CaretElement;
            if (inline == null)
            {
                paragraph.Inlines.Add(link);
                return;
            }
            var before = new Run(new TextRange(inline.ContentStart, Caret).Text);
            var after = new Run(new TextRange(inline.ContentEnd, Caret).Text);
            if (!string.IsNullOrEmpty(before.Text))
            {
                paragraph.Inlines.InsertBefore(inline, before);
                paragraph.Inlines.InsertAfter(before, link);
            }
            else paragraph.Inlines.Add(link);
            if (!string.IsNullOrEmpty(after.Text))
                paragraph.Inlines.InsertAfter(inline, after);
            paragraph.Inlines.Remove(inline);
            SaveToNodeVal();
        }
    }
}
