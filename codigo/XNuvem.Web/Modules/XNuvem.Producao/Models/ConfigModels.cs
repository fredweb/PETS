using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XNuvem.Producao.Models
{
    public class MetaProducaoViewModel
    {
        [Display(Name="Código interno")]
        public int? AbsEntry { get; set; }
        
        [Display(Name="Nome")]
        public string Name { get; set; }
        
        [Display(Name="Pacotes (Manhã)")]
        public double ManhaPacotes { get; set; }
        
        [Display(Name = "Total de vendas (Manhã)")]
        public double ManhaVendas { get; set; }

        [Display(Name = "Pacotes (Tarde)")]
        public double TardePacotes { get; set; }
        
        [Display(Name = "Total de vendas (Tarde)")]
        public double TardeVendas { get; set; }

        [Display(Name = "Pacotes (Noite)")]
        public double NoitePacotes { get; set; }

        [Display(Name = "Total de vendas (Noite)")]
        public double NoiteVendas { get; set; }
    }
}