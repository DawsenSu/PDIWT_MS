using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PDIWT_MS_CZ.Converters
{
    public class StatusToPicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string _status = (string)value;
            if (_status == PDIWT_MS_CZ.Properties.Resources.Status_Success)
                return @"../Image/Icons/Ok.ico";
            if(_status == Properties.Resources.Status_Fail)
                return @"../Image/Icons/Error.ico";
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
