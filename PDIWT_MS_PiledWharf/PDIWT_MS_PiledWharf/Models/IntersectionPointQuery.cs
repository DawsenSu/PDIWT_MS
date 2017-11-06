using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;
using Bentley.GeometryNET;

namespace PDIWT_MS_PiledWharf.Models
{
    using Soil;

    public class IntersectionPointQuery
    {
        public List<BSplineSurfaceElement> FindBSElement()
        {
            List<BSplineSurfaceElement> _bsEleList = new List<BSplineSurfaceElement>();

            ScanCriteria _sc = new ScanCriteria();
            _sc.SetModelRef(Program.GetActiveDgnModelRef());
            BitMask _bsmask = new BitMask(false);
            _bsmask.Set((uint)MSElementType.BsplineSurface - 1);
            //uint _bmsize = 1 + (uint)MSElementType.BsplineSurface;
            //_bmsize = (_bmsize + 7) / 8;
            //_bmsize = (_bmsize * 16) - 15;
            _bsmask.EnsureCapacity(113);
            _sc.SetElementTypeTest(_bsmask);
            _sc.SetModelSections(DgnModelSections.GraphicElements);
            _sc.Scan((ele, modelref) =>
            {
                var eletype = ele.ElementType;
                _bsEleList.Add((BSplineSurfaceElement)ele);
                return StatusInt.Success;
            });
            return _bsEleList;
        }

        public void DrawInsectionPoint()
        {
            var _bslist = FindBSElement();
            foreach (var _bs in _bslist)
            {
                MSBsplineSurface _bsGeo = _bs.GetBsplineSurface();
            }
        }
    }
}
