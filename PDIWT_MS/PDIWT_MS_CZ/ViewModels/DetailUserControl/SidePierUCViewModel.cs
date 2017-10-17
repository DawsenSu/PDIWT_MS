using System;

using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using PDIWT_MS_CZ.Models;

namespace PDIWT_MS_CZ.ViewModels.DetailUserControl
{
    public class SidePierUCViewModel : ViewModelBase
    {
        public SidePier CZ_SidePier
        {
            get { return GetProperty(() => CZ_SidePier); }
            set { SetProperty(() => CZ_SidePier, value); }
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            CZ_SidePier = new SidePier()
            {
                PierHeight = 1,
                PierXY_A = 2,
                PierXY_B = 3,
                PierXY_C = 4,
                PierXY_D = 5,
                PierXY_E = 6,
                PierXY_F = 7,
                IsChamfered = true,
                PierChamfer_Tx = 8,
                PierChamfer_Ty = 9,
                PierChamfer_R = 10
            };
        }
    }
}