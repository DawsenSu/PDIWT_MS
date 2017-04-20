using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PDIWT_MS_CZ.Models
{
    public class GSProperty
    {
        [Required/*,ReadOnly(true)*/]
        public string IntervalType { get; set; }
        [Required]
        public double Interval { get; set; }

    }
}
