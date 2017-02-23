using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SmartGrid
{
    class TagTemplateSelector: DataTemplateSelector
    {
        public DataTemplate EconomyMiddleClassDataTemplate { get; set; }
        public DataTemplate BuisnessPremiumClassDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = (FrameworkElement)container;
            var tag = item as HeadNode;
            if (tag != null) return (DataTemplate)element.FindResource("HeadNodeTemplate");
            return (DataTemplate)element.FindResource("NodeTemplate");

        }
    }
}
