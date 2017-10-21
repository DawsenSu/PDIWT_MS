using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Brush = System.Drawing.Brush;

namespace PDIWT_MS_CZ.Converters
{
    public class EdgeToForegroundConverter:IValueConverter
    {
        public SolidColorBrush XBrush { get; set; }
        public SolidColorBrush YBrush { get; set; }
        public SolidColorBrush ZBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int edge = (int) value;
            if (edge <= 3)
                return XBrush;
            else if (edge>=8)
            {
                return ZBrush;
            }
            else
            {
                return YBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
