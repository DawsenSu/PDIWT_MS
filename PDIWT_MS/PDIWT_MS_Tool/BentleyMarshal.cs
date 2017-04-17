using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace PDIWT_MS.Marshal
{
    public static class BentleyMarshal
    {
        /// <summary>
        /// 根据提供的ElementDescrp找到对应元素的各个属性
        /// </summary>
        /// <param name="volume">元素体积</param>
        /// <param name="area">元素面积</param>
        /// <param name="closureError">元素闭合误差</param>
        /// <param name="centroid">元素重心点</param>
        /// <param name="moment">元素力矩</param>
        /// <param name="ixy">xy方向惯性积</param>
        /// <param name="ixz">xz方向惯性积</param>
        /// <param name="iyz">yz方向惯性积</param>
        /// <param name="principalMoments">主惯量</param>
        /// <param name="principalDirections">主方向</param>
        /// <param name="edp">元素描述符</param>
        /// <param name="tolerance">误差</param>
        /// <returns>0返回成功，1返回失败</returns>
        [DllImport("ustation.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int mdlMeasure_volumeProperties(out double volume, out double area, out double closureError, out Bentley.GeometryNET.DPoint3d centroid, out Bentley.GeometryNET.DPoint3d moment, out double ixy, out double ixz, out double iyz, out Bentley.GeometryNET.DPoint3d principalMoments, out Bentley.GeometryNET.DPoint3d principalDirections, IntPtr edp, double tolerance);

        /// <summary>
        /// 通过elemRef获得elmdscr
        /// </summary>
        /// <param name="elemDescrPP">从元素引用获得的元素描述符</param>
        /// <param name="elemRef">元素描述符</param>
        /// <param name="modelRef">模型引用</param>
        /// <param name="expandSharedCells">必须为0,</param>
        /// <param name="filePos">文件位置</param>
        /// <returns>成功为0,失败为1</returns>
        [DllImport("ustation.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint mdlElmdscr_getByElemRef(out IntPtr elemDescrPP, IntPtr elemRef, IntPtr modelRef, int expandSharedCells, IntPtr filePos);

        [DllImport("ustation.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint mdlCell_getLibraryObject(out IntPtr librargObj, string LibName, bool unused);

        [DllImport("ustation.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void mdlCell_getLibraryName(out string filename);
    }
}
