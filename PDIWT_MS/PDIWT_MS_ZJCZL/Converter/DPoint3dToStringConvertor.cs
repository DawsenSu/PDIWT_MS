using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PDIWT_MS_ZJCZL.Converter
{
    class DPoint3dToStringConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var p = (Bentley.GeometryNET.DPoint3d)value;
            return string.Format("({0:f2},{1:f2},{2:f2})", p.X/10, p.Y/10, p.Z/10);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
