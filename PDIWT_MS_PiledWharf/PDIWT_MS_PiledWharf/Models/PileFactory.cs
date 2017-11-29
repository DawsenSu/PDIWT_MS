using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Bentley.DgnPlatformNET.DgnEC;
using Bentley.ECObjects.Instance;
using Bentley.GeometryNET;

namespace PDIWT_MS_PiledWharf.Models
{
    using Piles;
    using Piles.CrossSection;
    using Extension.Attribute;
    using Interface;

    public class PileFactory
    {
        public PileFactory(DPoint3d top, DPoint3d bottom, long id, IDgnECInstance piledgnecinstance)
        {
            double uorpm = Program.GetActiveDgnModel().GetModelInfo().UorPerMeter;
            m_toppoint  = top;
            m_bottompoint = bottom;
            m_toppoint.ScaleInPlace(1 / uorpm);
            m_bottompoint.ScaleInPlace(1 / uorpm);
            m_id = id;
            m_piledgnecinstance = piledgnecinstance;
        }

        IDgnECInstance m_piledgnecinstance;
        DPoint3d m_toppoint;
        DPoint3d m_bottompoint;
        long m_id;

        public PileBase Create()
        {
            string  _typevalue = m_piledgnecinstance.GetString("PileType");
            double? _weight = m_piledgnecinstance.GetDouble("PileWeight");
            double? _underwaterweight = m_piledgnecinstance.GetDouble("PileUnderWaterWeight");
            string _grid_horizontal = m_piledgnecinstance.GetString("PileGridHorizontal");
            string _grid_vertical = m_piledgnecinstance.GetString("PileGridVertical");

            //! 根据类型不断完善
            if(_typevalue == typeof(SolidPile).GetCustomAttribute<EnumDisplayNameAttribute>().DisplayName)
            {
                return new SolidPile()
                {
                    TopPoint = m_toppoint,
                    BottomPoint = m_bottompoint,
                    Code = _grid_horizontal + "-" + _grid_vertical,
                    ID = m_id,
                    ICrossSection = CreatePileCrossSection(),
                    UnitWeight = _weight.GetValueOrDefault(),
                    UnderWaterUnitWeight = _underwaterweight.GetValueOrDefault()
                };
            }
            else
            {
                return new SteelPCPile()
                {
                    TopPoint = m_toppoint,
                    BottomPoint = m_bottompoint,
                    Code = _grid_horizontal + "-" + _grid_vertical,
                    ID = m_id,
                    ICrossSection = CreatePileCrossSection(),
                    UnitWeight = _weight.GetValueOrDefault(),
                    UnderWaterUnitWeight = _underwaterweight.GetValueOrDefault()
                };
            }
        }

        IPileCrossSection CreatePileCrossSection()
        {
            string _crosssectiontype = m_piledgnecinstance.GetString("PileCrossSection");
            double? _sidelength = m_piledgnecinstance.GetDouble("SideLength");
            double? _innerDiameter = m_piledgnecinstance.GetDouble("PileInnerDiameter");

            //! 根据类型不断完善
            if(_crosssectiontype == typeof(AnnularCrossSection).GetCustomAttribute<EnumDisplayNameAttribute>().DisplayName)
            {
                return new AnnularCrossSection(_sidelength.GetValueOrDefault(), _innerDiameter.GetValueOrDefault());
            }
            else if(_crosssectiontype == typeof(SquareCrossSection).GetCustomAttribute<EnumDisplayNameAttribute>().DisplayName)
            {
                return new SquareCrossSection(_sidelength.GetValueOrDefault());
            }
            else
            {
                return new SquareWithRoundHoleCrossSection(_sidelength.GetValueOrDefault(), _innerDiameter.GetValueOrDefault());
            }
            //else if(_crosssectiontype == typeof(PolygonCrossSection).GetCustomAttribute<EnumDisplayNameAttribute>().DisplayName)
            //{
            //    return
            //}
        }
    }

}
