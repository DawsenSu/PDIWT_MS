using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using PDIWT_MS_PiledWharf.Interface;
using System.Collections.ObjectModel;
using PDIWT_MS_PiledWharf.Extension.Attribute;
using Bentley.GeometryNET;

namespace PDIWT_MS_PiledWharf.Models.Piles.CrossSection
{
    [EnumDisplayName(DisplayName = "多边形")]
    public class PolygonCrossSection : ObservableObject, IPileCrossSection
    {
        public PolygonCrossSection(ObservableCollection<DPoint3d> points)
        {
            _Points = points;
            m_polygon = new Polygon(_Points);
        }

        private ObservableCollection<DPoint3d> _Points;
        public ObservableCollection<DPoint3d> Points
        {
            get { return _Points; }
            private set { Set(ref _Points, value); }
        }
        private Polygon m_polygon;
        
        public double GetActualSectionArea(double fraction)
        {
            DPoint3d _centroid;
            double area;
            m_polygon.CentroidAreaXY(out _centroid, out area);
            return area;
        }

        public double GetOutPerimeter(double fraction)
        {
            return m_polygon.SumOfLengths();
        }

        public double GetBottomSectionArea()
        {
            return GetActualSectionArea(1);
        }
    }
}
