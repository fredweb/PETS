using System.Collections.Generic;
using XNuvem.Data;
using XNuvem.Dominio.Entidade.Base;

namespace XNuvem.Dominio.Entidade
{
    public class EspecieRecord:BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public virtual ICollection<RacaRecord> Racas {get;set;}
    }

    public class EspecieMap : EntityMap<EspecieRecord>
    {
        public EspecieMap()
        {
            Table("ESPECIEANIMAL");
            Id(i => i.Id).Column("IDESPECIE").GeneratedBy.Increment();
            Map(m => m.Nome).Column("NMNOME").Length(500).Not.Nullable();
            Map(m => m.Sigla).Column("SGESPECIE").Length(5).Not.Nullable();
        }
    }
}