using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCHXCodeQueryLib;
using PDIWT_MS_ZJCZL.Interface;
using DevExpress.Mvvm;

namespace PDIWT_MS_ZJCZL.Models
{
    public class RoundnessPileGeometry : BindableBase,IPileProperty
    {
        public Point3d PileBottomPoint
        {
            get { return GetProperty(() => PileBottomPoint); }
            set { SetProperty(() => PileBottomPoint, value); }
        }
        public double PileDiameter
        {
            get { return GetProperty(() => PileDiameter); }
            set { SetProperty(() => PileDiameter, value); }
        }
        public double PileInnerDiameter
        {
            get { return GetProperty(() => PileInnerDiameter); }
            set { SetProperty(() => PileInnerDiameter, value); }
        }
        public Point3d PileTopPoint
        {
            get { return GetProperty(() => PileTopPoint); }
            set { SetProperty(() => PileTopPoint, value); }
        }
        public double PileUnderWaterWeight
        {
            get { return GetProperty(() => PileUnderWaterWeight); }
            set { SetProperty(() => PileUnderWaterWeight, value); }
        }
        public double PileWeight
        {
            get { return GetProperty(() => PileWeight); }
            set { SetProperty(() => PileWeight, value); }
        }
        public double WaterLevel
        {
            get { return GetProperty(() => WaterLevel); }
            set { SetProperty(() => WaterLevel, value); }
        }

        public double GetCosAlpha()
        {
            return Math.Abs(PileTopPoint.Z - PileBottomPoint.Z) / GetPileLength();
        }

        public double GetPileCrossSectionArea()
        {
            return Math.PI * Math.Pow(PileDiameter / 2, 2);
        }

        public double GetPileGravity()
        {
            if (WaterLevel >= PileTopPoint.Z)
                return GetPileLength() * GetPileCrossSectionArea() * PileUnderWaterWeight;
            else if (WaterLevel<= PileBottomPoint.Z)
                return GetPileLength() * GetPileCrossSectionArea() * PileWeight;
            else
            {
                double zlength = Math.Abs(PileTopPoint.Z - PileBottomPoint.Z);
                return GetPileCrossSectionArea() * (Math.Abs(PileTopPoint.Z - WaterLevel) * PileWeight + Math.Abs(WaterLevel - PileBottomPoint.Z) * PileUnderWaterWeight)/zlength;
            }
        }

        public double GetPileLength()
        {
            return Math.Sqrt(Math.Pow(PileTopPoint.X - PileBottomPoint.X, 2) + Math.Pow(PileTopPoint.Y - PileBottomPoint.Y, 2) + Math.Pow(PileTopPoint.Z - PileBottomPoint.Z, 2));
        }

        public double GetPilePerimeter()
        {
            return Math.PI * PileDiameter;
        }
    }
}
