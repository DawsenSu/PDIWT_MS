using System;
using System.Collections.Generic;
using Bentley.GeometryNET;

using PDIWT_MS_CZ.Models;

using DevExpress.Mvvm;

namespace PDIWT_MS_CZ.ViewModels.DetailUserControl
{
    public class ZPlanEmptyBoxUCViewModel : ViewModelBase
    {
        public ZPlanEmptyBox CZ_ZPlanEmptyBox
        {
            get { return GetProperty(() => CZ_ZPlanEmptyBox); }
            set { SetProperty(() => CZ_ZPlanEmptyBox, value); }
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            CZ_ZPlanEmptyBox = new ZPlanEmptyBox()
            {
                XDis = 1,
                YDis = 2,
                ZDis = 3,
                EmptyBoxHeight = 4,
                ZPlanInfos = new List<ZPlanInfo>()
                {
                    new ZPlanInfo()
                    {
                        Point2D =  new DPoint2d(0,0),
                        BoxEdgeChamferInfo= new EmptyBoxEdgeChameferInfo()
                        {
                            EdgeIndicator = 0,
                            IsChamfered = true,
                            ChamferLength = 1,
                            ChamferWidth = 1
                        }
                    },
                    new ZPlanInfo()
                    {
                        Point2D =  new DPoint2d(1,0),
                        BoxEdgeChamferInfo = new EmptyBoxEdgeChameferInfo()
                        {
                            EdgeIndicator = 1,
                            IsChamfered = true,
                            ChamferLength = 2,
                            ChamferWidth = 2
                        }
                    },
                    new ZPlanInfo()
                    {
                        Point2D =  new DPoint2d(1,1),
                        BoxEdgeChamferInfo = new EmptyBoxEdgeChameferInfo()
                        {
                            EdgeIndicator = 2,
                            IsChamfered = true,
                            ChamferLength = 3,
                            ChamferWidth = 3
                        }
                    },
                                        new ZPlanInfo()
                    {
                        Point2D =  new DPoint2d(0,1),
                        BoxEdgeChamferInfo = new EmptyBoxEdgeChameferInfo()
                        {
                            EdgeIndicator = 3,
                            IsChamfered = true,
                            ChamferLength = 4,
                            ChamferWidth = 4
                        }
                    }
                }
            };
        }
    }
}