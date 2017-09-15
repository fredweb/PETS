using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XNuvem.Vendas.Models
{
    public class AtividadeViewModel
    {
        [Required]
        public string DocType { get; set; }

        [Required]
        public int DocNum { get; set; }

        [Required]
        public string CardCode { get; set; }

        [Required]
        public string Comments { get; set; }
    }
}