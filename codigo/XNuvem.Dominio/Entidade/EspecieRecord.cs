using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class EspecieRecord
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
    }

    public class EspecieMap : EntityMap<EspecieRecord>
    {
        public EspecieMap()
        {
            Table("ESPECIEANIMAL");
            Id(i => i.Id).Column("SQESPECIE").GeneratedBy.Identity();
            Map(m => m.Nome).Column("NMNOME").Length(500).Not.Nullable();
            Map(m => m.Sigla).Column("SGESPECIE").Length(5).Not.Nullable();
        }
    }
}