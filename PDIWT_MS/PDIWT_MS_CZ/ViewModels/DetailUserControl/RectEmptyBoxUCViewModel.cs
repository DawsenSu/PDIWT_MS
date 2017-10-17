using System;
using System.Collections.Generic;
using DevExpress.Mvvm;
using PDIWT_MS_CZ.Models;

namespace PDIWT_MS_CZ.ViewModels.DetailUserControl
{
    public class RectEmptyBoxUCViewModel : ViewModelBase
    {
        public RectEmptyBox CZ_RectEmptyBox
        {
            get { return GetProperty(() => CZ_RectEmptyBox); }
            set { SetProperty(() => CZ_RectEmptyBox, value); }
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            CZ_RectEmptyBox = new RectEmptyBox()
            {
                XDis = 1,
                YDis = 2,
                ZDis = 3,
                EmptyBoxLength = 4,
                EmptyBoxWidth = 5,
                EmptyBoxHeight = 6,
                ChamferInfos = new List<EmptyBoxEdgeChameferInfo>()
                {
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 0,
                        ChamferLength = 0,
                        ChamferWidth = 0
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 1,
                        ChamferLength = 1,
                        ChamferWidth = 1
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 2,
                        ChamferLength = 2,
                        ChamferWidth = 2
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 3,
                        ChamferLength = 3,
                        ChamferWidth = 3
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 4,
                        ChamferLength = 4,
                        ChamferWidth = 4
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 5,
                        ChamferLength = 5,
                        ChamferWidth = 5
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 6,
                        ChamferLength = 6,
                        ChamferWidth = 6
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 7,
                        ChamferLength = 7,
                        ChamferWidth = 7
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 8,
                        ChamferLength = 8,
                        ChamferWidth = 8
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 9,
                        ChamferLength = 9,
                        ChamferWidth = 9
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 10,
                        ChamferLength = 10,
                        ChamferWidth = 10
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 11,
                        ChamferLength = 11,
                        ChamferWidth = 11
                    }
                }
            };
        }
    }
}