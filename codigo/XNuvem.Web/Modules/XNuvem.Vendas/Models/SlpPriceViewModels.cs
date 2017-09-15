using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XNuvem.Vendas.Models
{
    public class PriceItem
    {
        [Display(Name = "Item")]
        public string ItemCode { get; set; }

        [Display(Name = "Nome do item")]
        public string ItemName { get; set; }

        [Display(Name = "Formação de preço")]
        public double Price { get; set; }

        [Display(Name = "Desconto aceitável")]
        public double PriceDsc { get; set; }
    }

    public class SlpPriceViewModel
    {
        [Display(Name="Vendedor")]
        [Required(ErrorMessage = "É necessário informar o vendedor.")]
        public int SlpCode { get; set; }

        [Display(Name = "Nome do vendedor")]
        public string SlpName { get; set; }
        public IList<PriceItem> Items { get; set; }
    }
}