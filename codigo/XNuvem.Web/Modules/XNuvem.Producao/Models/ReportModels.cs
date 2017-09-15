using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XNuvem.Producao.Models
{
    public class ProdutividadeViewModel
    {
        [Display(Name="De")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }        

        [Display(Name="Até")]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [Display(Name = "Meta")]
        [Required]
        public string MetaAbs { get; set; }

        [Display(Name="Turno")]
        [Required]
        public string Turno { get; set; }

        public IList<ProdutividadeItemViewModel> Items { get; set; }
        public ProdutividadeGeralViewModel Geral { get; set; }
        public IList<ProdutividadeFamiliaItem> Familias { get; set; }
    }

    public class ProdutividadeItemViewModel
    {
        public string Familia { get; set; }
        public string Turno { get; set; }
        public string Grupo { get; set; }
        public double Quantity { get; set; }
        public double TotalCost { get; set; }
        public double TotalVendas { get; set; }
        public double TotalPacotes { get; set; }
        public double TotalKilo { get; set; }
        public double MetaCPV { get; set; }
        public double MetaPacotes { get; set; }
        public double MetaVendas { get; set; }
        
        public double PercCPV {
            get {
                if (TotalVendas == 0) return 0;
                return 100 * (TotalCost / TotalVendas);
            }
        }

        public double VarMetaCPV {
            get {
                return PercCPV - MetaCPV;
            }
        }

        public double VarMetaPCT {
            get {
                return TotalPacotes - MetaPacotes;
            }
        }

        public double PercMetaPCT {
            get {
                if (MetaPacotes == 0) return 0;
                return 100 * (TotalPacotes / MetaPacotes);
            }
        }

        public double VarMetaVenda {
            get {
                return TotalVendas - MetaVendas;
            }
        }

        public double PercMetaVenda {
            get {
                if (MetaVendas == 0) return 0;
                return 100 * (TotalVendas / MetaVendas);
            }
        }
    }

    public class ProdutividadeGeralViewModel
    {
        public double Quantity { get; set; }
        public double TotalCost { get; set; }
        public double TotalVendas { get; set; }
        public double TotalPacotes { get; set; }
        public double TotalKilo { get; set; }
    }

    public class ProdutividadeFamiliaItem
    {
        public string Familia { get; set; }
        public double Quantity { get; set; }
        public double TotalCost { get; set; }
        public double TotalVendas { get; set; }
        public double TotalPacotes { get; set; }
        public double TotalKilo { get; set; }
        public double MetaPacotes { get; set; }
        public double MetaVendas { get; set; }

        public double PercCPV {
            get {
                if (TotalVendas == 0) return 0;
                return 100 * (TotalCost / TotalVendas);
            }
        }

        public double VarMetaPCT {
            get {
                return TotalPacotes - MetaPacotes;
            }
        }

        public double PercMetaPCT {
            get {
                if (MetaPacotes == 0) return 0;
                return 100 * (TotalPacotes / MetaPacotes);
            }
        }

        public double VarMetaVenda {
            get {
                return TotalVendas - MetaVendas;
            }
        }

        public double PercMetaVenda {
            get {
                if (MetaVendas == 0) return 0;
                return 100 * (TotalVendas / MetaVendas);
            }
        }

        public IList<ProdutividadeTurnoItem> Items { get; set; }
    }

    public class ProdutividadeTurnoItem
    {
        public string Familia { get; set; }
        public string Turno { get; set; }
        public double Quantity { get; set; }
        public double TotalCost { get; set; }
        public double TotalVendas { get; set; }
        public double TotalPacotes { get; set; }
        public double TotalKilo { get; set; }
        public double MetaPacotes { get; set; }
        public double MetaVendas { get; set; }
        public double PercCPV {
            get {
                if (TotalVendas == 0) return 0;
                return 100 * (TotalCost / TotalVendas);
            }
        }

        public double VarMetaPCT {
            get {
                return TotalPacotes - MetaPacotes;
            }
        }

        public double PercMetaPCT {
            get {
                if (MetaPacotes == 0) return 0;
                return 100 * (TotalPacotes / MetaPacotes);
            }
        }

        public double VarMetaVenda {
            get {
                return TotalVendas - MetaVendas;
            }
        }

        public double PercMetaVenda {
            get {
                if (MetaVendas == 0) return 0;
                return 100 * (TotalVendas / MetaVendas);
            }
        }

        public IList<ProdutividadeItemViewModel> Items { get; set; }
    }
}