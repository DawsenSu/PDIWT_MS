using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

using BD = Bentley.DgnPlatformNET;
using BG = Bentley.GeometryNET;

using HCHXCodeQueryLib;

using PDIWT_MS_ZJCZL.Models;

namespace PDIWT_MS_ZJCZL.ViewModels
{
    public class ViewAddPileViewModel : ViewModelBase
    {

        public ObservableCollection<PileInfoClass> Piles
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
        //public Dictionary<PileType,string> PileTypeDic
        //{
        //    get
        //    {
        //        var r = new Dictionary<PileType, string>();
        //        foreach (var item in Enum.GetValues(typeof(PileType)))
        //        {
        //            r.Add(item,EnumSourceHelper)
        //        }
        //    }
        //}
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
        public void AddPile()
        {
            var temppileinfo = new PileInfoClass() { PileId = -1, PileDiameter = this.PileDiameter, PileTypeInfo = this.PileTypeInfo, PileCode = this.PileCode };

            //Piles.Add(new PileInfoClass
            //{
            //    PileId = 100,
            //    PileCode = "Test3",
            //    PileDiameter = 15,
            //    PileTypeInfo = PileType.Socketed,
            //    SoilInfo = new ObservableCollection<SoilInfoClass>()
            //        {
            //            new SoilInfoClass() { SoilLayerName="layer5", SoilLayerNum="2-0" },
            //            new SoilInfoClass() { SoilLayerName="layer6", SoilLayerNum="2-1" }
            //        },
            //    CalParameter = new CalculateParameter { GammaR = 1.5 },
            //    Result = new CalculateResult { UltimateBearingCapacity = 400, UltimatePullingCapacity = 400 }
            //}
            //);
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            PileDiameter = 2.3; PileTypeInfo = PileType.Filling; PileCode = "A1";
            GammaR = 1.2;
        }

    }
}