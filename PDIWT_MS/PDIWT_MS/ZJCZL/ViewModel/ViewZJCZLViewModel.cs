using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

using HCHXCodeQueryLib;


namespace PDIWT_MS.ZJCZL.ViewModel
{
    public class ViewZJCZLViewModel : ViewModelBase
    {
        protected override void OnInitializeInDesignMode()
        {
            base.OnInitializeInDesignMode();
            PilesInfo = new ObservableCollection<PileInfo>()
            {
                new PileInfo { PileID="1", PileCode="code1" }
            };
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            PilesInfo = new ObservableCollection<PileInfo>()
            {
                new PileInfo
                {
                    PileID = "1", PileCode = "Test",
                    SoilInfo = new List<SoilInfoClass>
                    {
                        new SoilInfoClass() { SoilLayerName="layer1", SoilLayerNum="0-0" },
                        new SoilInfoClass() { SoilLayerName="layer2", SoilLayerNum="0-1" }
                    }
                },
                new PileInfo
                {
                    PileID = "2", PileCode = "Test2" ,
                    SoilInfo = new List<SoilInfoClass>
                    {
                        new SoilInfoClass() { SoilLayerName="layer2", SoilLayerNum="0-2" },
                        new SoilInfoClass() { SoilLayerName="layer3", SoilLayerNum="0-3" }
                    }
                }
            };
        }
        public virtual ObservableCollection<PileInfo> PilesInfo
        {
            get { return GetProperty(() => PilesInfo); }
            private set { SetProperty(() => PilesInfo, value); }
        }
    }

    public class PileInfo
    {
        [Category("PileInfo"), Description("桩在dgn文件中ID号"),DisplayName("桩ID")]
        public string PileID { get; set; }
        [Category("PileInfo"), Description("桩的文件编号"),DisplayName("桩编码")]
        public string PileCode { get; set; }
        [Category("SoildInfo"), Description("土层信息"),DisplayName("土层信息"),TypeConverter(typeof(ExpandableObjectConverter))]
        public List<SoilInfoClass> SoilInfo { get; set; }
        //public ColumnLayerInfoArray SoilInfo { get; set; }

    }
    public class SoilInfoClass
    {
        [Description("土层编号"), DisplayName("土层编号")]
        public string SoilLayerNum { get; set; }
        [Description("土层名称"), DisplayName("土层名称")]
        public string SoilLayerName { get; set; }
        public override string ToString()
        {
            return SoilLayerName;
        }
    }
}