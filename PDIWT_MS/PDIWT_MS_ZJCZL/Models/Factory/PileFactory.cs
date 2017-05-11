using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HCHXCodeQueryLib;
using PDIWT_MS_ZJCZL.Models.Piles;
using PDIWT_MS_ZJCZL.Interface;

namespace PDIWT_MS_ZJCZL.Models.Factory
{
    public abstract class PileFactory
    {
        protected HCHXCodeQuery pileQuery = new HCHXCodeQuery();
        protected ColumnLayerInfoArray columnLayerInfoArray = new ColumnLayerInfoArray(); //之后应由华创生成不同的土层类
        public abstract PileBase CreateNewPile(IPileProperty pileType, string pilecode, long pileId);
        public abstract PileBase[] CreateNewPileArray(IPileProperty[] pileTypeArray, string[] pileCodeArray, long[] pileIdArray);
    }
}
