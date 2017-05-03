using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_ZJCZL.Models
{
    public static class EnumBindingToCHSHelper
    {
        public static string GetEnumDesc(Enum e)
        {
            FieldInfo enuminfo = e.GetType().GetField(e.ToString());
            DescriptionAttribute enumAttribute = enuminfo.GetCustomAttribute<DescriptionAttribute>();
            if (enumAttribute != null)
                return enumAttribute.Description;
            else
                return e.ToString();
            
        }

        public  static IList ToList(Type type)
        {
            if (type == null) throw new ArgumentException("Type");
            if (!type.IsEnum) throw new ArgumentException("Type provieded must be an Enum.", "Type");

            ArrayList list = new ArrayList();
            Array array = type.GetEnumValues();
            foreach (Enum value in array)
            {
                list.Add(new KeyValuePair<Enum, string>(value, GetEnumDesc(value)));
            }
            return list;
        }
        
        public static List<string> GetEnumDescriptionList(Type type)
        {
            if (type == null) throw new ArgumentException("Type");
            if (!type.IsEnum) throw new ArgumentException("Type provieded must be an Enum.", "Type");

            List<string> enumdescriptionlist = new List<string>();
            Array array = type.GetEnumValues();
            foreach (Enum item in array)
            {
                enumdescriptionlist.Add(GetEnumDesc(item));
            }
            return enumdescriptionlist;
        }

        public static Enum GetEnumByDescription(string desc, Type type)
        {
            if (type == null) throw new ArgumentException("Type");
            if (!type.IsEnum) throw new ArgumentException("Type provieded must be an Enum.", "Type");
            if (string.IsNullOrEmpty(desc)) throw new ArgumentException("Description must not be null or empty");

            Array array = type.GetEnumValues();
            foreach (Enum item in array)
            {
                if (GetEnumDesc(item) == desc) return item;
            }
            return null;
        }
    }
}
