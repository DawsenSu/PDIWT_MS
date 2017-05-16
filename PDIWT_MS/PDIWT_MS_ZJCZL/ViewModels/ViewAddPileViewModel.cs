using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

using BD = Bentley.DgnPlatformNET;
using BG = Bentley.GeometryNET;

using HCHXCodeQueryLib;

using PDIWT_MS_ZJCZL.Models;
using PDIWT_MS_ZJCZL.Models.Piles;

namespace PDIWT_MS_ZJCZL.ViewModels
{
    public class ViewAddPileViewModel : ViewModelBase
    {

        public ObservableCollection<PileBase> Piles
        {
            get { return GetProperty(() => Piles); }
            set { SetProperty(() => Piles, value); }
        }
        public Point3d StartPoint
        {
            get { return GetProperty(() => StartPoint); }
            set { SetProperty(() => StartPoint, value); }
        }
        public Point3d EndPoint
        {
            get { return GetProperty(() => EndPoint); }
            set { SetProperty(() => EndPoint, value); }
        }
        public double PileDiameter
        {
            get { return GetProperty(() => PileDiameter); }
            set { SetProperty(() => PileDiameter, value); }
        }
        public PileType PileTypeInfo
        {
            get { return GetProperty(() => PileTypeInfo); }
            set { SetProperty(() => PileTypeInfo, value); }
        }
        public string PileCode
        {
            get { return GetProperty(() => PileCode); }
            set { SetProperty(() => PileCode, value); }
        }
        public double GammaR
        {
            get { return GetProperty(() => GammaR); }
            set { SetProperty(() => GammaR, value); }
        }


        [Command]
        //public void AddPile()
        //{
        //    var temppileinfo = new PileInfoClass() { PileId = -1, PileDiameter = this.PileDiameter, PileTypeInfo = this.PileTypeInfo, PileCode = this.PileCode };
        //    temppileinfo.PileLength = GetLengthByVertex(StartPoint, EndPoint);

        //    //查找桩基所在土层
        //    ColumnLayerInfoArray columnLayerInfoArray = new ColumnLayerInfoArray();
        //    double pointscale = 1e4;
        //    Point3d calculatestartpoint = Point3dScale(StartPoint, pointscale), calculateendpoint = Point3dScale(EndPoint, pointscale);
        //    HCHXCodeQueryErrorCode status = m_pileQuery.QueryByRay(ref columnLayerInfoArray, calculatestartpoint, calculateendpoint);

        //    if (status != HCHXCodeQueryErrorCode.Success)
        //    {
        //        System.Windows.MessageBox.Show($"查找出现错误!\n{status}", "查找出现错误", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        //        return;
        //    }
        //    var columnLayerList = columnLayerInfoArray.GetSortedColumnLayerList();

        //    temppileinfo.SoilInfo = new ObservableCollection<SoilInfoClass>();
        //    var random = new Random();
        //    for (int i = 0; i < columnLayerList.Count; i++)
        //    {
        //        double pileinsoilLenght;
        //        if (i == columnLayerList.Count - 1)
        //            pileinsoilLenght = GetLengthByVertex(columnLayerList[i].TopPosition, calculateendpoint) / pointscale;
        //        else
        //            pileinsoilLenght = GetLengthByVertex(columnLayerList[i].TopPosition, columnLayerList[i + 1].TopPosition) / pointscale;
        //        temppileinfo.SoilInfo.Add(new SoilInfoClass(columnLayerList[i].IntersectLayerInfo.Category, columnLayerList[i].IntersectLayerInfo.UserCode, pileinsoilLenght, Math.Round(random.NextDouble() * 10 + 100, 2), Math.Round(random.NextDouble() * 10 * +100, 2)));
        //    }

        //    //计算桩基承载力
        //    temppileinfo.CalParameter = new CalculateParameter { GammaR = this.GammaR };

        //    temppileinfo.Result = CalculatePileCapacity.Calculate(PileDiameter, temppileinfo.SoilInfo, temppileinfo.CalParameter);

        //    Piles.Add(temppileinfo);
        //}

        double GetLengthByVertex(Point3d startPoint, Point3d endPoint)
        {
            return Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2) + Math.Pow(startPoint.Z - endPoint.Z, 2));
        }
        Point3d Point3dScale(Point3d point, double scale)
        {
            point.X *= scale; point.Y *= scale; point.Z *= scale;
            return point;
        }

        //public CalculateResult Calculate(double diameter, ObservableCollection<SoilInfoClass> soilinfo, CalculateParameter calparmeter)
        //{
        //    double pilearea = Math.PI * Math.Pow(diameter / 2, 2);
        //    double pileperimeter = Math.PI * diameter;
        //    double accumlatenum = 0;
        //    foreach (var pileeachlength in soilinfo)
        //    {
        //        accumlatenum += pileeachlength.Length * pileeachlength.Qfi;
        //    }
        //    double result = (pileperimeter * accumlatenum + soilinfo.Last().Qr * pilearea) / calparmeter.GammaR;
        //    return new CalculateResult { UltimateBearingCapacity = result, UltimatePullingCapacity = -1 };

        //}

        [Command]
        public void SelectPileAxes()
        {
            SelectPileTool.InstallNewInstance();
        }

        Point3d DPoint3dToPoint3d(BG.DPoint3d p)
        {
            return new Point3d() { X = p.X, Y = p.Y, Z = p.Z };
        }


        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            PileDiameter = 2.3; PileTypeInfo = PileType.Solid; PileCode = "A1";
            GammaR = 1.2; StartPoint = new Point3d { X = -134.2015, Y = 68.1099, Z = 78.9160 }; EndPoint = new Point3d { X = -134.2015, Y = 68.1099, Z = -126.4281 };
        }

        #region Field
        HCHXCodeQuery m_pileQuery = new HCHXCodeQuery();
        #endregion

    }
}