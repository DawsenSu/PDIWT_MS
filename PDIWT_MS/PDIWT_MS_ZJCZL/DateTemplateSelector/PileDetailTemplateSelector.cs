using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PDIWT_MS_ZJCZL.Models.Piles;

namespace PDIWT_MS_ZJCZL.DateTemplateSelector
{
    class PileDetailTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var pile = item as PileBase;
            if (pile!=null)
            {
                if (pile is SolidPile)
                    return (container as FrameworkElement).FindResource("SoildPileDetailTemplate") as DataTemplate;
                if (pile is FillingPile)
                    return (container as FrameworkElement).FindResource("FillingPileDetailTemplate") as DataTemplate;
            }
            return null;
        }
    }
}
