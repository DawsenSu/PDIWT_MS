using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

using HCHXCodeQueryLib;
using PDIWT_MS_ZJCZL.Models;

namespace PDIWT_MS_ZJCZL.ViewModels
{
    public class ViewZJCZLViewModel :ViewModelBase
    {

        public PileInfoClass Pile
        {
            get { return GetProperty(() => Pile); }
            set { SetProperty(() => Pile, value); }
        }
        #region SearchPanel
        public List<SearchPanel> SPanel
        {
            get { return GetProperty(() => SPanel); }
            set { SetProperty(() => SPanel, value); }
        }

        public string SearchId
        {
            get { return GetProperty(() => SearchId); }
            set { SetProperty(() => SearchId, value); }
        }

        public string StartPoint
        {
            get { return GetProperty(() => StartPoint); }
            set { SetProperty(() => StartPoint, value); }
        }

        public string EndPoint
        {
            get { return GetProperty(() => EndPoint); }
            set { SetProperty(() => EndPoint, value); }
        }

        public string Info
        {
            get { return GetProperty(() => Info); }
            set { SetProperty(() => Info, value); }
        }
        #endregion
        [Command]
        public void SearchById()
        {
            Info = string.Empty;
            AllLayerInfo idInfo = new AllLayerInfo();
            HCHXCodeQueryErrorCode status = PileQuery.QueryById(ref idInfo, SearchId);
            if (status != HCHXCodeQueryErrorCode.Success)
            {
                System.Windows.MessageBox.Show($"查找出现错误!\n{status}", "查找出现错误", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (idInfo.BaseInfo.Count == 0)
                return;
            Pile = new PileInfoClass();
            Pile.SoilInfo = new ObservableCollection<SoilInfoClass>();
            foreach (KeyValuePair<string,CodeInfo> codeInfo in idInfo.BaseInfo)
            {
                Pile.PileId = double.Parse(codeInfo.Value.ID);
                Pile.PileCode = codeInfo.Value.ToString();

                ColumnLayerInfoArray columnLayerInfo;
                if (!idInfo.IntersectLayerInfos.TryGetValue(codeInfo.Key, out columnLayerInfo))
                    continue;
                foreach (var layerInfo in columnLayerInfo.m_layers)
                {
                    
                    Pile.SoilInfo.Add(new SoilInfoClass( layerInfo.IntersectLayerInfo.Category,layerInfo.IntersectLayerInfo.UserCode,layerInfo.TopPosition, layerInfo.BasePosition));
                }
            }
            Info = "查找完成";
        }
        public bool CanSearchById()
        {
            return !string.IsNullOrEmpty(SearchId);
        }
        [Command]
        public void SearchByRay()
        {
            Info = "";
            Point3d startPoint = ParseStringToPoint3d(StartPoint);
            Point3d endPoint = ParseStringToPoint3d(EndPoint);
            ColumnLayerInfoArray columnLayerInfo = new ColumnLayerInfoArray();
            HCHXCodeQueryErrorCode status = PileQuery.QueryByRay(ref columnLayerInfo, startPoint, endPoint);

            if (status != HCHXCodeQueryErrorCode.Success)
            {
                System.Windows.MessageBox.Show($"查找出现错误!\n{status}", "查找出现错误", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            Pile = new PileInfoClass();
            Pile.SoilInfo = new ObservableCollection<SoilInfoClass>();
            Pile.PileId = -1;
            Pile.PileCode = $"此为虚拟桩\n顶部坐标为:{StartPoint}\r\n底部坐标为:{EndPoint}\n";
            foreach (var columnInfo in columnLayerInfo.m_layers)
            {
                Pile.SoilInfo.Add(new SoilInfoClass(columnInfo.IntersectLayerInfo.Category, columnInfo.IntersectLayerInfo.UserCode, columnInfo.TopPosition, columnInfo.BasePosition));
            }
            Info = "查找完成";
        }
        public bool CanSearchByRay()
        {
            return !(string.IsNullOrEmpty(StartPoint) || string.IsNullOrEmpty(EndPoint));
        }

        #region Field
        HCHXCodeQuery PileQuery = new HCHXCodeQuery();
        #endregion

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            Pile = new PileInfoClass
            {
                PileId = 924,
                PileCode = "Test",
                SoilInfo = new ObservableCollection<SoilInfoClass>()
                {
                    new SoilInfoClass() { SoilLayerName="layer1", SoilLayerNum="0-0" },
                    new SoilInfoClass() { SoilLayerName="layer2", SoilLayerNum="0-1" }
                }
            };
            SPanel = new List<SearchPanel>()
            {
                new SearchPanel() { Name = "根据桩ID获取", Type = SearchType.ById },
                new SearchPanel() { Name = "根据虚拟射线获取", Type = SearchType.ByRay}
            };
            SearchId = "924";
            StartPoint = "(173928.573996,46617.477392,36875.226081)";
            EndPoint = "(28714.044367,130457.125163,-801521.251635)";
        }

        Point3d ParseStringToPoint3d(string pointstr)
        {
            string temp = pointstr.Trim('(', ')');
            Point3d point = new Point3d();
            string[] xyz = temp.Split(',');
            point.X = double.Parse(xyz[0]);
            point.Y = double.Parse(xyz[1]);
            point.Z = double.Parse(xyz[2]);
            return point;
        }
        //protected override void OnInitializeInDesignMode()
        //{
        //    base.OnInitializeInDesignMode();
        //    SoilInfo = new ObservableCollection<SoilInfoClass>
        //            {
        //                new SoilInfoClass() { SoilLayerName="layer1", SoilLayerNum="0-0" },
        //                new SoilInfoClass() { SoilLayerName="layer2", SoilLayerNum="0-1" }
        //            };
        //}
    }
}