using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using HCHXCodeQueryLib;

using PDIWT_MS_ZJCZL.Interface;
using PDIWT_MS_ZJCZL.Models.Piles;
using PDIWT_MS_ZJCZL.Models.Soil;
using PDIWT_MS_ZJCZL.Models.PileCrossSection;

namespace PDIWT_MS_ZJCZL.Models.Factory
{
    public class SteelAndPercastConcretePileFactory : PileFactory
    {
        public SteelAndPercastConcretePileFactory(double gammar)
        {
            m_gammar = gammar;
        }
        double m_gammar;
        public override PileBase CreateNewPile(IPileProperty pileType, string pilecode, long pileId)
        {
            if (!(pileType is AnnularPileGeometry))
            {
                throw new ArgumentOutOfRangeException("SteelAndPercastConcretePile must be Annular cross section!");
            }
            HCHXCodeQueryErrorCode status = pileQuery.QueryByRay(ref columnLayerInfoArray, pileType.PileTopPoint.Scale(1e4), pileType.PileBottomPoint.Scale(1e4));
            if (status != HCHXCodeQueryErrorCode.Success)
                throw new ArgumentException("创建" + pilecode + "出错" + status.ToString());
            var resultlayer = columnLayerInfoArray.GetSortedColumnLayerList();
            var tempLayerBase = new ObservableCollection<SoilLayerInfoBase>();
            double temppileLength;
            var random = new Random();
            for (int i = 0; i < resultlayer.Count; i++)
            {
                if (i == resultlayer.Count - 1)
                    temppileLength = resultlayer[i].TopPosition.Distance(pileType.PileBottomPoint.Scale(1e4));
                else
                    temppileLength = resultlayer[i].TopPosition.Distance(resultlayer[i + 1].TopPosition);
                temppileLength = Math.Round(temppileLength * 1e-4, 2);
                tempLayerBase.Add(new SoilLayerInfoBase() { SoilLayerName = resultlayer[i].IntersectLayerInfo.Category, SoilLayerNum = resultlayer[i].IntersectLayerInfo.UserCode, PileInSoilLayerLength = temppileLength, Qfi = random.NextDouble(), Xii = random.NextDouble() });
            }
            return new SteelAndPercastConcretePile() { PileCode = pilecode, PileId = pileId, PilePropertyInfo = pileType, SteelAndPercastConcretPileLayerInfoProp = tempLayerBase, GammaR = 1.5, Qr = 6000, Eta = 1.1 };
        }
    }
}