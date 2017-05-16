using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDIWT_MS_ZJCZL.Interface;
using PDIWT_MS_ZJCZL.Models.Piles;
using PDIWT_MS_ZJCZL.Models.Soil;
using HCHXCodeQueryLib;
using System.Collections.ObjectModel;
using PDIWT_MS_ZJCZL.Models.PileCrossSection;


namespace PDIWT_MS_ZJCZL.Models.Factory
{
    public class SolidPileFactory : PileFactory
    {
        public SolidPileFactory(double gammar)
        {
            m_gammar = gammar;
        }
        double m_gammar;
        public override PileBase CreateNewPile(IPileProperty pileType, string pilecode, long pileId)
        {
            if (!(pileType is SquarePileGeometry) && !(pileType is SquareWithRoundHolePileGeometry) && !(pileType is PolygonPileGeometry))
            {
                throw new ArgumentOutOfRangeException("SoildPile must be one of these pile cross section (square,square with round hole, polygon)");
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
                temppileLength = Math.Round(temppileLength*1e-4,2);
                tempLayerBase.Add(new SoilLayerInfoBase() { SoilLayerName = resultlayer[i].IntersectLayerInfo.Category, SoilLayerNum = resultlayer[i].IntersectLayerInfo.UserCode, PileInSoilLayerLength = temppileLength, Qfi = random.NextDouble(), Xii = random.NextDouble() });
            }
            return new SolidPile() { PileCode = pilecode, PileId = pileId, PilePropertyInfo = pileType, SolidPileSoilLayerInfoProp = tempLayerBase, GammaR = m_gammar, Qr = 6000 };
        }
    }
}
