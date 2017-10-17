using System;

using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using PDIWT_MS_CZ.Models;

namespace PDIWT_MS_CZ.ViewModels.DetailUserControl
{
    public class DoorSillUCViewModel : ViewModelBase
    {
        public DoorSill CZ_DoorSill
        {
            get { return GetProperty(() => CZ_DoorSill); }
            set { SetProperty(() => CZ_DoorSill, value); }
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            CZ_DoorSill = new DoorSill()
            {
                DoorSillHeight = 1,
                DoorSill_A = 2,
                DoorSill_B = 3,
                DoorSill_C = 4,
                DoorSill_D = 5,
                DoorSill_E = 6,
                DoorSill_F = 7
            };
        }
    }
}