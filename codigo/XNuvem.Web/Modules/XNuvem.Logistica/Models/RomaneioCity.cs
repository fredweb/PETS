using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XNuvem.Logistica.Models
{
    public class RomaneioCity
    {
        public int DocEntry { get; set; }
        public int LineId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double DocTotal { get; set; }
    }
}