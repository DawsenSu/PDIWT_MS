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
        public string Success { get; set; }
        public string Error { get; set; }
        public string Success_picpath { get; set; }
        public string Error_picpath { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string _status = (string)value;
            if (_status == Success)
                return Success_picpath;
            if(_status == Error)
                return Error_picpath;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
