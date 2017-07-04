using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace PDIWT_MS_Tool.Extension
{
    public static class ToolExtension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> inlist)
        {
            var collection = new ObservableCollection<T>();
            foreach (var item in inlist)
            {
                collection.Add(item);
            }
            return collection;
        }
    }
}
