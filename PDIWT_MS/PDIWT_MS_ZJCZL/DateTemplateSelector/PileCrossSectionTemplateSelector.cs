using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PDIWT_MS_ZJCZL.Interface;
using PDIWT_MS_ZJCZL.Models.PileCrossSection;

namespace PDIWT_MS_ZJCZL.DateTemplateSelector
{
    class PileCrossSectionTemplateSelector : DataTemplateSelector
    {
        public string RoumdnessTempKey { get; set; }
        public string SquareTempKey { get; set; }
        public string AnnualrTempKey { get; set; }
        public string SquareWithRoundHoleKey { get; set; }
        public string PolygonKey { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var pilecrosssection = item as IPileProperty;
            if (pilecrosssection!=null)
            {
                if(pilecrosssection is RoundnessPileGeometry)
                    return (container as FrameworkElement).FindResource(RoumdnessTempKey) as DataTemplate;
                if (pilecrosssection is SquarePileGeometry)
                    return (container as FrameworkElement).FindResource(SquareTempKey) as DataTemplate;
                if (pilecrosssection is AnnularPileGeometry)
                    return (container as FrameworkElement).FindResource(AnnualrTempKey) as DataTemplate;
                if (pilecrosssection is SquareWithRoundHolePileGeometry)
                    return (container as FrameworkElement).FindResource(SquareWithRoundHoleKey) as DataTemplate;
                if (pilecrosssection is PolygonPileGeometry)
                    return (container as FrameworkElement).FindResource(PolygonKey) as DataTemplate;
            }
            return null;
        }
    }
}
