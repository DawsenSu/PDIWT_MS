using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIWT_MS_CZ.Models
{

    public class HoleProperty
    {
        [Required]
        public double HoleLength { get; set; }
        [Required]
        public double HoleWidth { get; set; }
        [Required]
        public double HoleHeight { get; set; }
        [Required]
        public double XDis { get; set; }
        [Required]
        public double YDis { get; set; }
        [Required]
        public double ZDis { get; set; }
        [Required]
        public double ChamferLength { get; set; }
    }
}
