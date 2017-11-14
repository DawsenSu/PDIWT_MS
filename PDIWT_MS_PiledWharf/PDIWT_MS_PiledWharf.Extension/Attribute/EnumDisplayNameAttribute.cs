using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_PiledWharf.Extension.Attribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EnumDisplayNameAttribute : System.Attribute
    {
        public string DisplayName { get; set; }
    }
}
