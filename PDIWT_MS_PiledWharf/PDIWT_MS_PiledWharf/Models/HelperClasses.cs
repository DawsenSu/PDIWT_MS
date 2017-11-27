using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using System.Reflection;

namespace PDIWT_MS_PiledWharf.Models
{
    using Piles;
    using Piles.CrossSection;
    using Extension.Attribute;

    public static class EnumDisPlayNameHelper
    {
        public static List<string> GetPileTypeNameList()
        {
            return GetTypeEnumDisplayAttribute(new List<Type>() { typeof(SolidPile), typeof(SteelPCPile) });
        }

        public static List<string> GetPileCrossSectionTypeNameList()
        {
            return GetTypeEnumDisplayAttribute(new List<Type>
            {
                typeof(AnnularCrossSection),
                typeof(SquareCrossSection),
                typeof(SquareWithRoundHoleCrossSection),
                typeof(PolygonCrossSection)
            });
        }

        private static List<string> GetTypeEnumDisplayAttribute(IEnumerable<Type> typecollection)
        {
            List<string> _list = new List<string>();
            foreach (var type in typecollection)
            {
                var _attri = type.GetCustomAttribute<EnumDisplayNameAttribute>();
                if (_attri == null)
                    _list.Add(type.Name);
                else
                    _list.Add(_attri.DisplayName);
            }
            return _list;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> _list)
        {
            var _temp = new ObservableCollection<T>();
            foreach (var item in _list)
            {
                _temp.Add(item);
            }
            return _temp;
        }
    }


}
