using System;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Reflection;

namespace PDIWT_MS_PiledWharf.Extension.Converter
{
    public class InterfaceMethodReusltConverter : IValueConverter
    {
        public string InterfaceName { get; set; }
        public string MethodName { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type interfacetype = value.GetType().GetInterface(InterfaceName);
            if (interfacetype == null) return DependencyProperty.UnsetValue;
            MethodInfo methodinfo = interfacetype.GetMethod(MethodName);
            if (methodinfo == null) return DependencyProperty.UnsetValue;
            
            return methodinfo.Invoke(value,new object[] { double.Parse(parameter.ToString())});
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class PileCrossectionTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Dictionary<string, string> _crossectiontype_name_dic = new Dictionary<string, string>
            {
                {"AnnularCrossSection","环形" },
                {"PolygonCrossSection","多边形" },
                {"SquareCrossSection","方形" },
                {"SquareWithRoundHoleCrossSection","方形圆孔" }
            };
            return _crossectiontype_name_dic[value.GetType().Name];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

}
