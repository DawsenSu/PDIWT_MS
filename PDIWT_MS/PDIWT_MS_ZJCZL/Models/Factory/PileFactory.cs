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
        public PileBase[] CreateNewPileArray(IPileProperty[] pileTypeArray, string[] pileCodeArray, long[] pileIdArray)
        {
            if (pileTypeArray.Length != pileCodeArray.Length || pileTypeArray.Length != pileIdArray.Length)
                throw new ArgumentException("传入参数pileTypeArray,pileCodeArray,pileIdArray的长度不相等");
            var pilelist = new List<PileBase>();
            for (int i = 0; i < pileTypeArray.Length; i++)
            {
                pilelist.Add(CreateNewPile(pileTypeArray[i], pileCodeArray[i], pileIdArray[i]));
            }
            return pilelist.ToArray();
        }
    }
}
