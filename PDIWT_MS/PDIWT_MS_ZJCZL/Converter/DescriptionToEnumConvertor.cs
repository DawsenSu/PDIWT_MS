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
            //string description = string.Empty;
            //switch (pieltype)
            //{
            //    case PileType.Solid:
            //        description = "实心桩或桩端封闭";
            //        break;
            //    case PileType.SteelAndPercastConcrete:
            //        description = "管桩";
            //        break;
            //    case PileType.Filling:
            //        description = "灌注桩";
            //        break;
            //    case PileType.Socketed:
            //        description = "嵌岩桩";
            //        break;
            //    case PileType.PostgroutingFilling:
            //        description = "后注浆灌注桩";
            //        break;
            //}
            //return description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string description = (string)value;
            return EnumBindingToCHSHelper.GetEnumByDescription(description, targetType);
            //PileType piletype = PileType.Filling;
            //switch (description)
            //{
            //    case "实心桩或桩端封闭":
            //        piletype = PileType.Solid;
            //        break;
            //    case "管桩":
            //        piletype = PileType.SteelAndPercastConcrete;
            //        break;
            //    case "灌注桩":
            //        piletype = PileType.Filling;
            //        break;
            //    case "嵌岩桩":
            //        piletype = PileType.Socketed;
            //        break;
            //    case "后注浆灌注桩":
            //        piletype = PileType.PostgroutingFilling;
            //        break;
            //}
            //return piletype;
        }
    }
}
