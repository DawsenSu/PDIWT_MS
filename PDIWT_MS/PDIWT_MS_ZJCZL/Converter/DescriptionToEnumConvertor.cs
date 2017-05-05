using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using PDIWT_MS_ZJCZL.Models;

namespace PDIWT_MS_ZJCZL.Converter
{
    class DescriptionToEnumConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PileType pieltype = (PileType)value;
            return EnumBindingToCHSHelper.GetEnumDesc(pieltype);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string description = (string)value;
            return EnumBindingToCHSHelper.GetEnumByDescription(description, targetType);
        }
    }
}
