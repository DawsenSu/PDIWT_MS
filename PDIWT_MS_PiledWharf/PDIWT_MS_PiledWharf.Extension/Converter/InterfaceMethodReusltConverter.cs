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
    using Extension.Attribute;
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
            Type value_type = value.GetType();
            var attri = value_type.GetCustomAttribute<EnumDisplayNameAttribute>();
            return attri == null ? value_type.Name : attri.DisplayName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

}
