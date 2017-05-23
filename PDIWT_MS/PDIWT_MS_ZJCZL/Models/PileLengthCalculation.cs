using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BG = Bentley.GeometryNET;
using BD = Bentley.DgnPlatformNET;

using HCHXCodeQueryLib;
using MathNet.Numerics;

namespace PDIWT_MS_ZJCZL.Models
{
    using Piles;
    using Interface;
    using Factory;
    class PileLengthCalculation
    {
        public PileLengthCalculation(PileBase pile, double wantedPileBearing)
        {
            m_pile = pile;
            m_wantedPileBearing = wantedPileBearing;
        }
        PileBase m_pile;
        double m_wantedPileBearing;
        HCHXCodeQuery pileQuery = new HCHXCodeQuery();
        ColumnLayerInfoArray columnLayerInfoArray;

        //unit:nm
        double GetActiveModelBoundaryZ()
        {
            BD.DgnModel activemodel = Program.GetActiveDgnModel();
            BG.DRange3d modelrange;
            activemodel.GetRange(out modelrange);
            return modelrange.Low.Z;
        }
        //return-> unit:m
        //lowpointz-> unit:m
        BG.DPoint3d GetVirtualPileAxisBottomPoint(double lowpointz)
        {
            BG.DSegment3d pileaxis = new BG.DSegment3d(m_pile.PilePropertyInfo.PileTopPoint.Point3dToDPoint3d(), m_pile.PilePropertyInfo.PileBottomPoint.Point3dToDPoint3d());
            double pileaxiszlength = pileaxis.Extent.Z;
            double virtualaxiszlength = pileaxis.StartPoint.Z - lowpointz;
            double scale = Math.Abs(virtualaxiszlength / pileaxiszlength);
            return pileaxis.PointAtFraction(scale);
        }
        [Obsolete]
        void GetGetPileCalculateInfo(out Dictionary<double, double> pileLengthAndBearingForce)
        {
            pileLengthAndBearingForce = new Dictionary<double, double>();
            Point3d originalBottomPoint = new HCHXCodeQueryLib.Point3d()
            {
                X = m_pile.PilePropertyInfo.PileBottomPoint.X,
                Y = m_pile.PilePropertyInfo.PileBottomPoint.Y,
                Z = m_pile.PilePropertyInfo.PileBottomPoint.Z,
            };
            IPileProperty tempprop = m_pile.PilePropertyInfo;//set pile to the whole length pile
            tempprop.PileBottomPoint = GetVirtualPileAxisBottomPoint(2 * GetActiveModelBoundaryZ() * 1e-4).DPoint3dToPoint3d();
            List<double> eachlayerz = new List<double>();

            #region SolidPile
            if (m_pile is SolidPile)
            {
                var solidpile = m_pile as SolidPile;
                SolidPileFactory solidfactory = new SolidPileFactory(solidpile.GammaR);

                var tempsolidpile = solidfactory.CreateNewPile(tempprop, solidpile.PileCode, solidpile.PileId) as SolidPile;
                //double[] qfiliacummlate = new double[tempsolidpile.SolidPileSoilLayerInfoProp.Count];
                //double[] qri = new double[tempsolidpile.SolidPileSoilLayerInfoProp.Count];
                //double gammar = solidpile.GammaR;
                //double perimeter = solidpile.PilePropertyInfo.GetPilePerimeter();
                //double area = solidpile.PilePropertyInfo.GetPileOutLineArea();

                //for (int i = 0; i < tempsolidpile.SolidPileSoilLayerInfoProp.Count; i++)
                //{
                //    if (i == 0)
                //        qfiliacummlate[i] = tempsolidpile.SolidPileSoilLayerInfoProp[i].Qfi * tempsolidpile.SolidPileSoilLayerInfoProp[i].PileInSoilLayerLength;
                //    else
                //        qfiliacummlate[i] = qfiliacummlate[i - 1] + tempsolidpile.SolidPileSoilLayerInfoProp[i].Qfi * tempsolidpile.SolidPileSoilLayerInfoProp[i].PileInSoilLayerLength;
                //    qri[i] = tempsolidpile.SolidPileSoilLayerInfoProp[i].
                //}
                for (int i = 0; i < tempsolidpile.SolidPileSoilLayerInfoProp.Count; i++)
                {
                    if (i == 0)
                        eachlayerz.Add(tempsolidpile.SolidPileSoilLayerInfoProp[i].PileInSoilLayerTopZ);
                    else
                    {
                        eachlayerz.Add(tempsolidpile.SolidPileSoilLayerInfoProp[i].PileInSoilLayerTopZ + 1e-4);
                        eachlayerz.Add(tempsolidpile.SolidPileSoilLayerInfoProp[i].PileInSoilLayerTopZ - 1e-4);
                    }
                }
                eachlayerz.Add(2 * GetActiveModelBoundaryZ() * 1e-4); //添加底点

                for (int i = 0; i < eachlayerz.Count; i++)
                {
                    tempprop.PileBottomPoint = GetVirtualPileAxisBottomPoint(eachlayerz[i]).DPoint3dToPoint3d();
                    if (i == 0)
                        pileLengthAndBearingForce.Add(tempprop.GetPileLength(), 0);
                    else
                    {
                        tempsolidpile = solidfactory.CreateNewPile(tempprop, solidpile.PileCode, solidpile.PileId) as SolidPile;
                        double tempbearing = tempsolidpile.CalculateQd();
                        pileLengthAndBearingForce.Add(tempprop.GetPileLength(), tempbearing);
                        if (tempbearing > m_wantedPileBearing)
                            break;
                    }
                }
            }
            #endregion

            #region steelpile
            if (m_pile is SteelAndPercastConcretePile)
            {
                var steelandpercastconcretepile = m_pile as SteelAndPercastConcretePile;
                SteelAndPercastConcretePileFactory steelandpercastpilefactory = new SteelAndPercastConcretePileFactory(steelandpercastconcretepile.GammaR, steelandpercastconcretepile.Eta);
                var temppile = steelandpercastpilefactory.CreateNewPile(tempprop, steelandpercastconcretepile.PileCode, steelandpercastconcretepile.PileId) as SteelAndPercastConcretePile;
                //foreach (var layer in temppile.SteelAndPercastConcretPileLayerInfoProp)
                //{
                //    eachlayerz.Add(layer.PileInSoilLayerTopZ);
                //}
                for (int i = 0; i < temppile.SteelAndPercastConcretPileLayerInfoProp.Count; i++)
                {
                    if (i == 0)
                        eachlayerz.Add(temppile.SteelAndPercastConcretPileLayerInfoProp[i].PileInSoilLayerTopZ);
                    else
                    {
                        eachlayerz.Add(temppile.SteelAndPercastConcretPileLayerInfoProp[i].PileInSoilLayerTopZ + 1e-4);
                        eachlayerz.Add(temppile.SteelAndPercastConcretPileLayerInfoProp[i].PileInSoilLayerTopZ - 1e-4);
                    }
                }
                eachlayerz.Add(2 * GetActiveModelBoundaryZ() * 1e-4); //添加底点


                for (int i = 0; i < eachlayerz.Count; i++)
                {
                    tempprop.PileBottomPoint = GetVirtualPileAxisBottomPoint(eachlayerz[i]).DPoint3dToPoint3d();
                    if (i == 0)
                        pileLengthAndBearingForce.Add(tempprop.GetPileLength(), 0);
                    else
                    {
                        temppile = steelandpercastpilefactory.CreateNewPile(tempprop, steelandpercastconcretepile.PileCode, steelandpercastconcretepile.PileId) as SteelAndPercastConcretePile;
                        double tempbearing = temppile.CalculateQd();
                        pileLengthAndBearingForce.Add(tempprop.GetPileLength(), tempbearing);
                        if (tempbearing > m_wantedPileBearing)
                            break;
                    }

                }
            }
            #endregion

            tempprop.PileBottomPoint = originalBottomPoint;//setback
        }

        void GetPileCacluateInfo(out Dictionary<double, double> pilelengthbearingforce)
        {
            try
            {
                IPileProperty temppropinfo = m_pile.PilePropertyInfo.Clone() as IPileProperty;
                temppropinfo.PileBottomPoint = GetVirtualPileAxisBottomPoint(1.2 * GetActiveModelBoundaryZ() * 1e-4).DPoint3dToPoint3d();
                pilelengthbearingforce = new Dictionary<double, double>(); //输出参数

                HCHXCodeQueryErrorCode status = pileQuery.QueryByRay(ref columnLayerInfoArray, temppropinfo.PileTopPoint.Scale(1e4), temppropinfo.PileBottomPoint.Scale(1e4));
                if (columnLayerInfoArray.m_layers.Count == 0)
                    status = HCHXCodeQueryErrorCode.NoIntersection;
                if (status != HCHXCodeQueryErrorCode.Success)
                    throw new ArgumentException("创建出错:" + status.ToString());
                var resultlayer = columnLayerInfoArray.GetSortedColumnLayerList();

                #region SoildPile
                if (m_pile is SolidPile)
                {
                    SolidPile solidpile = m_pile as SolidPile;
                    double[] qfiliacummlate = new double[resultlayer.Count];
                    double[] qri = new double[resultlayer.Count];
                    double gammar = solidpile.GammaR;
                    double perimeter = solidpile.PilePropertyInfo.GetPilePerimeter();
                    double area = solidpile.PilePropertyInfo.GetPileOutLineArea();

                    for (int i = 0; i < resultlayer.Count; i++)
                    {
                        qri[i] = resultlayer[i].IntersectLayerInfo.Qri;
                        if (i == 0)
                            qfiliacummlate[i] = resultlayer[i].IntersectLayerInfo.Qfi * (resultlayer[i].TopPosition.Distance(resultlayer[i + 1].TopPosition) * 1e-4);
                        else if (i == resultlayer.Count - 1)
                            qfiliacummlate[i] = qfiliacummlate[i - 1] + resultlayer[i].IntersectLayerInfo.Qfi * (resultlayer[i].TopPosition.Distance(temppropinfo.PileBottomPoint.Scale(1e4)) * 1e-4);
                        else
                            qfiliacummlate[i] = qfiliacummlate[i - 1] + resultlayer[i].IntersectLayerInfo.Qfi * (resultlayer[i].TopPosition.Distance(resultlayer[i + 1].TopPosition) * 1e-4);
                    }
                    pilelengthbearingforce.Add(resultlayer[0].TopPosition.Distance(temppropinfo.PileTopPoint.Scale(1e4)) * 1e-4, 0);
                    for (int i = 0; i < qfiliacummlate.Length - 1; i++)
                    {
                        pilelengthbearingforce.Add((resultlayer[i + 1].TopPosition.Distance(temppropinfo.PileTopPoint.Scale(1e4)) - 1) * 1e-4, (perimeter * qfiliacummlate[i] + qri[i] * area) / gammar);
                        pilelengthbearingforce.Add((resultlayer[i + 1].TopPosition.Distance(temppropinfo.PileTopPoint.Scale(1e4)) + 1) * 1e-4, (perimeter * qfiliacummlate[i] + qri[i + 1] * area) / gammar);
                    }
                    pilelengthbearingforce.Add(temppropinfo.GetPileLength(), (perimeter * qfiliacummlate.Last() + qri.Last() * area) / gammar);
                }
                if (m_pile is SteelAndPercastConcretePile)
                {
                    SteelAndPercastConcretePile spcpile = m_pile as SteelAndPercastConcretePile;
                    double[] qfiliacummlate = new double[resultlayer.Count];
                    double[] qriWitheta = new double[resultlayer.Count];
                    double gammar = spcpile.GammaR;
                    double eta = spcpile.Eta;
                    double perimeter = spcpile.PilePropertyInfo.GetPilePerimeter();
                    double area = spcpile.PilePropertyInfo.GetPileOutLineArea();

                    for (int i = 0; i < resultlayer.Count; i++)
                    {
                        qriWitheta[i] = resultlayer[i].IntersectLayerInfo.Qri * eta;
                        if (i == 0)
                            qfiliacummlate[i] = resultlayer[i].IntersectLayerInfo.Qfi * (resultlayer[i].TopPosition.Distance(resultlayer[i + 1].TopPosition) * 1e-4);
                        else if (i == resultlayer.Count - 1)
                            qfiliacummlate[i] = qfiliacummlate[i - 1] + resultlayer[i].IntersectLayerInfo.Qfi * (resultlayer[i].TopPosition.Distance(temppropinfo.PileBottomPoint.Scale(1e4)) * 1e-4);
                        else
                            qfiliacummlate[i] = qfiliacummlate[i - 1] + resultlayer[i].IntersectLayerInfo.Qfi * (resultlayer[i].TopPosition.Distance(resultlayer[i + 1].TopPosition) * 1e-4);
                    }
                    pilelengthbearingforce.Add(resultlayer[0].TopPosition.Distance(temppropinfo.PileTopPoint.Scale(1e4)) * 1e-4, 0);
                    for (int i = 0; i < qfiliacummlate.Length - 1; i++)
                    {
                        pilelengthbearingforce.Add((resultlayer[i + 1].TopPosition.Distance(temppropinfo.PileTopPoint.Scale(1e4)) - 1) * 1e-4, (perimeter * qfiliacummlate[i] + qriWitheta[i] * area) / gammar);
                        pilelengthbearingforce.Add((resultlayer[i + 1].TopPosition.Distance(temppropinfo.PileTopPoint.Scale(1e4)) + 1) * 1e-4, (perimeter * qfiliacummlate[i] + qriWitheta[i + 1] * area) / gammar);
                    }
                    pilelengthbearingforce.Add(temppropinfo.GetPileLength(), (perimeter * qfiliacummlate.Last() + qriWitheta.Last() * area) / gammar);
                }
                #endregion
            }
            catch
            {
                throw;
            }

        }
        public string TestString()
        {
            Dictionary<double, double> pilelengthandbearingforce;
            GetPileCacluateInfo(out pilelengthandbearingforce);
            StringBuilder sb = new StringBuilder();
            foreach (var info in pilelengthandbearingforce)
            {
                sb.Append($"Length:{info.Key}  bearingforce:{info.Value}\n");
            }
            return sb.ToString();
        }
        public double GetPileLengthByBearingCapacity()
        {
            if (m_wantedPileBearing <= 0)
                throw new ArgumentOutOfRangeException("m_wantedPileBearing");
            Dictionary<double, double> pilelengthandbearingforce;
            GetPileCacluateInfo(out pilelengthandbearingforce);
            List<double> points = new List<double>(), values = new List<double>();
            foreach (var keyvalue in pilelengthandbearingforce)
            {
                points.Add(keyvalue.Key);
                values.Add(keyvalue.Value);
            }
            var interpolate = Interpolate.Linear(points, values);

            return FindRoots.OfFunction(x => interpolate.Interpolate(x) - m_wantedPileBearing, points.Min(), points.Max());
        }



    }
}
