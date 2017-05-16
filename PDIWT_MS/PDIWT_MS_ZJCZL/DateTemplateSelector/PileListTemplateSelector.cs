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
    class PileListTemplateSelector : DataTemplateSelector
    {
        public string SoildPileKey { get; set; }
        public string SteelAndPercastConcretePileKey { get; set; }
        public string FillingPileKey { get; set; }
        public string SocketedPileKey { get; set; }
        public string PostgroutingFillingPileKey { get; set; }
        public string NullPileKey { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var pile = item as PileBase;
            if (pile!=null)
            {
                if (pile is SolidPile)
                    return (container as FrameworkElement).FindResource(SoildPileKey) as DataTemplate;
                if (pile is SteelAndPercastConcretePile)
                    return (container as FrameworkElement).FindResource(SteelAndPercastConcretePileKey) as DataTemplate;
                if (pile is FillingPile)
                    return (container as FrameworkElement).FindResource(FillingPileKey) as DataTemplate;
                if (pile is SocketedPile)
                    return (container as FrameworkElement).FindResource(SocketedPileKey) as DataTemplate;
                if (pile is PostgroutingFillingPile)
                    return (container as FrameworkElement).FindResource(PostgroutingFillingPileKey) as DataTemplate; 
            }
            return (container as FrameworkElement).FindResource(NullPileKey) as DataTemplate;
        }
    }
}
