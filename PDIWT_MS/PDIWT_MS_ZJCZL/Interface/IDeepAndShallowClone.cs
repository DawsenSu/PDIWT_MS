using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_ZJCZL.Interface
{
    interface IDeepAndShallowClone<T>
    {
        T DeepClone();
        T ShallowClone();
    }
}
