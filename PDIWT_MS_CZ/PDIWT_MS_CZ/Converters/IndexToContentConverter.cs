using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PDIWT_MS_CZ.Converters
{
    public class IndexToContentConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = (byte) value;
            return index == 0 ?
                (UserControl)new Views.DetailUserControl.ShortCulvertUserControl() 
                : (UserControl)new Views.DetailUserControl.LocalConcertationCulvertUserControl() ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
