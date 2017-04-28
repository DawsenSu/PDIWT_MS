using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using HCHXCodeQueryLib;


namespace PDIWT_MS_ZJCZL.Converter
{
    class Point3dToStringConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var point = (Point3d)value;
            return string.Format("{0:n4},{1:n4},{2:n4}", point.X, point.Y, point.Z);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] xyz = ((string)value).Split(',');
            return new Point3d() { X = double.Parse(xyz[0]), Y = double.Parse(xyz[1]), Z = double.Parse(xyz[2]) };
        }
    }
}
