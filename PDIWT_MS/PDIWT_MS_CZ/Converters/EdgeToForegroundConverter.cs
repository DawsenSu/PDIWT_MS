using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PDIWT_MS_CZ.Converters
{
    class EdgeToForegroundConverter:IValueConverter
    {
        public SolidBrush XBrush { get; set; }
        public SolidBrush YBrush { get; set; }
        public SolidBrush ZBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int edge = (int) value;
            if (edge <= 4)
                return XBrush;
            else if (edge>=9)
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
            throw new NotImplementedException();
        }
    }
}
