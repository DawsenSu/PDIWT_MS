using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Serialization;

namespace PDIWT_MS_CZ.Models
{
    public class GSProperty
    {
        [XmlElement(ElementName = "尺寸类型")]
        [Required/*,ReadOnly(true)*/]
        public string IntervalType { get; set; }
        [XmlElement(ElementName = "尺寸长度")]
        [Required]
        public double Interval { get; set; }

    }
}
