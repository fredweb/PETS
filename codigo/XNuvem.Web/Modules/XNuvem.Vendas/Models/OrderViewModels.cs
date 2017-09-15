/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using XNuvem.Vendas.DomainModels;

namespace XNuvem.Vendas.Models
{
    public class OrderViewModel
    {
        [Display(Name="Nº do lançamento")]
        public int iDocEntry { get; set; }

        [Display(Name="Nº do documento", Description="Nº do documento no SAP")]
        public int DocEntry { get; set; }
        
        [Display(Name="Data de lançamento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DocDate { get; set; }
        
        [Display(Name="Cliente")]
        [Required(ErrorMessage="O campo código do cliente não pode estar vazio.")]
        public string CardCode { get; set; }

        [Required]
        [Display(Name="Condição de pagamento")]
        public int GroupNum { get; set; }

        [Display(Name="Condição de pagamento")]
        public string PymntGroup { get; set; }

        [Required]
        [Display(Name="Forma de pagamento")]
        public string PeyMethod { get; set; }

        [Display(Name="Lista de preço")]
        public string ListName { get; set; }
        
        public IEnumerable<OrderLineViewModel> Lines { get; set; }

        [Required]
        [Display(Name = "Rota")]
        public string RotaCode { get; set; }

        [Display(Name="Nome da rota")]
        public string RotaName { get; set; }

        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        /// <summary>
        /// Business partner for edit
        /// </summary>
        public BusinessPartnerResume BusinessPartner { get; set; }

        public CreditBalance FinancialDetails { get; set; }

        public bool Approved { get; set; }

        [Required]
        [Display(Name="Utilização")]
        public int Usage { get; set; }

        [Display(Name="Utilização")]
        public string UsageText { get; set; }

        [Display(Name="Carga")]
        public string Carga { get; set; }
    }

    public class OrderLineViewModel
    {
        [Display(Name="Grupo")]
        public string iItemGroup { get; set; }

        [Display(Name="Item")]
        [Required(ErrorMessage="O campo código do item não pode estar vazio.")]
        public string ItemCode { get; set; }

        [Display(Name="Nome do item")]
        [Required(ErrorMessage="O campo nome do item não pode estar vazio.")]
        public string ItemName { get; set; }
        
        [Display(Name="Preço")]
        [Required(ErrorMessage="O campo preço não pode estar vazio.")]
        [Range(1d, Double.MaxValue, ErrorMessage="Informe um valor maior que zero.")]
        public double Price { get; set; }

        [Display(Name="Quantidade")]
        [Required(ErrorMessage="O campo quantidade não pode estar vazio.")]
        [Range(1d, Double.MaxValue, ErrorMessage="Informe um valor maior que zero.")]
        public double Quantity { get; set; }
    }

    public class ItemGroupViewModel
    {
        public IList<ItemGroup> Groups { get; set; }
    }
}