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
using System.Text.RegularExpressions;

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
            if (columnLayerInfoArray.m_layers.Count == 0)
                status = HCHXCodeQueryErrorCode.NoIntersection;
            if (status != HCHXCodeQueryErrorCode.Success)
                throw new ArgumentException("创建" + pilecode + "出错:" + status.ToString());
            var resultlayer = columnLayerInfoArray.GetSortedColumnLayerList();
            var tempLayerBase = new ObservableCollection<SoilLayerInfoBase>();
            double temppileLength;
            var random = new Random();
            string soilName, soilNum;
            Regex regex = new Regex(@"\[.*\]");
            for (int i = 0; i < resultlayer.Count; i++)
            {
                if (i == resultlayer.Count - 1)
                    temppileLength = resultlayer[i].TopPosition.Distance(pileType.PileBottomPoint.Scale(1e4));
                else
                    temppileLength = resultlayer[i].TopPosition.Distance(resultlayer[i + 1].TopPosition);
                temppileLength = Math.Round(temppileLength*1e-4,2);
                Match mc = regex.Match(resultlayer[i].IntersectLayerInfo.UserCode);
                soilName = soilNum = resultlayer[i].IntersectLayerInfo.UserCode;
                if (mc.Success)
                {
                    soilName = mc.Value.Trim(new char[] { '[', ']' });
                    soilNum = soilNum.Replace(mc.Value, string.Empty);
                }
                tempLayerBase.Add(new SoilLayerInfoBase() { SoilLayerName = soilName, SoilLayerNum = soilNum, PileInSoilLayerLength = temppileLength, PileInSoilLayerTopZ = Math.Round(resultlayer[i].TopPosition.Z*1e-4,2), Qfi = resultlayer[i].IntersectLayerInfo.Qfi, Xii = resultlayer[i].IntersectLayerInfo.Xii });
            }
            return new SolidPile() { PileCode = pilecode, PileId = pileId, PilePropertyInfo = pileType, SolidPileSoilLayerInfoProp = tempLayerBase, GammaR = m_gammar, Qr = resultlayer.Last().IntersectLayerInfo.Qri };
        }
    }
}
