using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_PiledWharf.Interface
{
    public interface IPileCrossSection
    {
        //获得某一位置的桩的外界面周长
        double GetOutPerimeter(double fraction); //从桩顶到桩底fraction[0,1]
        //获得某一位置的桩的界面面积
        double GetActualSectionArea(double fraction);
        // 获得桩端的面积
        double GetBottomSectionArea();

    }
}
