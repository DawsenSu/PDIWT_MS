using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PDIWT_MS_CZ.Converters
{
    public class StatusToBackgroundConverter : IValueConverter
    {
        public string FristString { get; set; }
        public Brush FirstBackground { get; set; }
        public Brush SecondBackground { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string _str = (string)value;
            return FristString == _str ? FirstBackground : SecondBackground;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
