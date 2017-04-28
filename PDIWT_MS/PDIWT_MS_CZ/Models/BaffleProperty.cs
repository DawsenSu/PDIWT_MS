using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace PDIWT_MS_CZ.Models
{
    public sealed class BaffleProperty
    {
        [Required, XmlElement( ElementName = "消力坎宽度")]
        public double Width { get; set; }
        [Required, XmlElement( ElementName = "消力坎高度度")]
        public double Height { get; set; }
        [Required, XmlElement( ElementName = "消力坎距闸首中轴线距离")]
        public double XDis { get; set; }
    }
}
