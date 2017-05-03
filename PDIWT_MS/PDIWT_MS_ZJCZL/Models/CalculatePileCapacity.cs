using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PDIWT_MS_ZJCZL.Models
{
    public static class CalculatePileCapacity
    {
        public static CalculateResult Calculate(double diameter, ObservableCollection<SoilInfoClass> soilinfo, CalculateParameter calparmeter)
        {
            double pilearea = Math.PI * Math.Pow(diameter / 2, 2);
            double pileperimeter = Math.PI * diameter;
            double accumlatenum = 0;
            foreach (var pileeachlength in soilinfo)
            {
                accumlatenum += pileeachlength.Length * pileeachlength.Qfi;
            }
            double result = (pileperimeter * accumlatenum + soilinfo.Last().Qr * pilearea) / calparmeter.GammaR;
            return new CalculateResult { UltimateBearingCapacity = result, UltimatePullingCapacity = -1 };
        }
    }
}
