using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PDIWT_MS_CZ.Models
{
    public class HoleProperty
    {
        [XmlElement(ElementName = "空箱Y方向长度")]
        [Required]
        public double HoleLength { get; set; }
        [XmlElement(ElementName = "空箱X方向长度")]
        [Required]
        public double HoleWidth { get; set; }
        [XmlElement(ElementName = "空箱Z方向长度")]
        [Required]
        public double HoleHeight { get; set; }
        [XmlElement(ElementName = "空箱距边墩X轴方向距离")]
        [Required]
        public double XDis { get; set; }
        [XmlElement(ElementName = "空箱距边墩Y轴方向距离")]
        [Required]
        public double YDis { get; set; }
        [XmlElement(ElementName = "空箱距边墩Z轴方向距离")]
        [Required]
        public double ZDis { get; set; }
        [XmlElement(ElementName = "空箱倒角长")]
        [Required]
        public double ChamferLength { get; set; }
    }
}
