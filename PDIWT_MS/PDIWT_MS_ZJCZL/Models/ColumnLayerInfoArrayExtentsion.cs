using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HCHXCodeQueryLib;

namespace PDIWT_MS_ZJCZL.Models
{
    public static class ColumnLayerInfoArrayExtentsion
    {
        public static List<ColumnLayerInfo> GetSortedColumnLayerList(this ColumnLayerInfoArray colarray)
        {
            var layerList = from layer in colarray.m_layers
                            orderby layer.BasePosition.Z descending
                            select layer;
            return layerList.ToList();
        }
    }
}
