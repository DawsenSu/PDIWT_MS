using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

using PDIWT_MS_CZ.Models;

namespace PDIWT_MS_CZ.ViewModels.DetailUserControl
{
    public class BaseBoardUCViewModel : ViewModelBase
    {
        public BaseBoard CZ_BaseBoard
        {
            get { return GetProperty(() => CZ_BaseBoard); }
            set { SetProperty(() => CZ_BaseBoard, value); }
        }
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            CZ_BaseBoard = new BaseBoard()
            {
                BaseBoardLength = 100,
                BaseBoardWidth = 200,
                BaseBoardHeight = 300,
                EntranceWidth = 400,
                IsGrooving = true,
                IGrooving = new ShapeIGrooving(),
                TGrooving = new ShapeTGrooving()
                {
                    GroovingHeight = 0,
                    GroovingBackLength = 1,
                    GroovingFrontLength = 2,
                    GroovingWidth = 3,
                    GroovingGradient = 4
                }
            };
        }
    }
}