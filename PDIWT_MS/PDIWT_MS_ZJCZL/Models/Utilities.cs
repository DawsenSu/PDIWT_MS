using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_ZJCZL.Models
{
    using PDIWT_MS_ZJCZL.Interface;

    public class Utilities
    {
        public static string GetPileSkewnessString(double cosa)
        {
            if (Math.Abs(cosa - 1) < 1e-4)
            {
                return "直桩";
            }
            else
            {
                return string.Format("1:{0}", Math.Round(1.0 / Math.Sqrt(1 - cosa * cosa) / cosa, 0));
            }
        }
        /// <summary>
        /// 根据inerval规定的间隔，向上取值
        /// </summary>
        /// <param name="numbers">想要取整的数</param>
        /// <param name="interval">间隔值，如按0.5取值</param>
        /// <returns>向上取整后的数组</returns>
        public static double CellingWithInterval(double numbers, double interval)
        {
            if (interval <=0 )
            {
                throw new ArgumentOutOfRangeException("interval");
            }
            return Math.Ceiling(numbers / interval) * interval;
        }


    }

}
