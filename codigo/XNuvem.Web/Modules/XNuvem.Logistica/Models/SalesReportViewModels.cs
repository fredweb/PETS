using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XNuvem.Logistica.Models.SalesReportViewModels
{
    public class SalesReportCitiesModel
    {
        [Display(Name = "Representante", Description="Nenhum vendedor para todos os vendedores")]
        public int SlpCode { get; set; }

        [Display(Name = "Nome do representante")]
        public string SlpName { get; set; }

        [Display(Name = "De")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }

        [Display(Name = "Até")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }
    }

    public class CityReport
    {
        public string HasData { get; set; }
        #region Periodo properties...
        // Propriedades do período informado
        
        public string PIbgeCode { get; set; }
        public string PState { get; set; }
        public string PCity { get; set; }
        public int PPopulacao { get; set; }
        public double PLat { get; set; }
        public double PLng { get; set; }
        public double PDocTotal { get; set; }
        #endregion

        #region Ultimos 3 meses properties...
        // Propriedades dos meses fixos
        
        public string SIbgeCode { get; set; }
        public string SState { get; set; }
        public string SCity { get; set; }
        public int SPopulacao { get; set; }
        public double SLat { get; set; }
        public double SLng { get; set; }
        public double SDocTotal { get; set; }
        #endregion
    }
}