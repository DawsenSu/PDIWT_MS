using System;
using DevExpress.Mvvm;

using PDIWT_MS_CZ.Models;

namespace PDIWT_MS_CZ.ViewModels.DetailUserControl
{
    public class ShortCulvertUCViewModel : ViewModelBase
    {
        public ShortCulvert CZ_ShortCulvert
        {
            get { return GetProperty(() => CZ_ShortCulvert); }
            set { SetProperty(() => CZ_ShortCulvert, value); }
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            CZ_ShortCulvert = new ShortCulvert()
            {
                Culvert_Pier_BackDis = 1,
                Culvert_Width = 2,
                Culvert_A = 3,
                Culvert_B = 4,
                Culvert_C = 5,
                Culvert_D = 6,
                Culvert_R1 = 7,
                Culvert_R2 = 8
            };
        }
    }
}