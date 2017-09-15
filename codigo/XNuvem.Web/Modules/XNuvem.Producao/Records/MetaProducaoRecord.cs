using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XNuvem.Data;

namespace XNuvem.Producao.Records
{
    public class MetaProducaoRecord
    {
        public int AbsEntry { get; set; }
        public string Name { get; set; }
        public double ManhaPacotes { get; set; }
        public double ManhaVendas { get; set; }
        public double TardePacotes { get; set; }
        public double TardeVendas { get; set; }
        public double NoitePacotes { get; set; }
        public double NoiteVendas { get; set; }
    }

    public class MetaProducaoMap : EntityMap<MetaProducaoRecord>
    {
        public MetaProducaoMap() {
            Table("MetasProducao");

            Id(m => m.AbsEntry).GeneratedBy.Identity();
            Map(m => m.Name).Length(100).Not.Nullable();
            Map(m => m.ManhaPacotes).Not.Nullable();
            Map(m => m.ManhaVendas).Not.Nullable();
            Map(m => m.TardePacotes).Not.Nullable();
            Map(m => m.TardeVendas).Not.Nullable();
            Map(m => m.NoitePacotes).Not.Nullable();
            Map(m => m.NoiteVendas).Not.Nullable();
        }
    }
}