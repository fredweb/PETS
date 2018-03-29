using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class TipoDocumentoRecord : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
    }

    public class TipoDocumentoMap : EntityMap<TipoDocumentoRecord>
    {
        public TipoDocumentoMap()
        {
            Table("TIPODOCUMENTO");
            Id(i => i.Id).Column("IDTIPODOCUMENTO").GeneratedBy.Increment();
            Map(m => m.Nome).Column("NMNOME").Length(500).Not.Nullable();
            Map(m => m.Sigla).Column("SGTIPODOCUMENTO").Length(4).Not.Nullable();
        }
    }
}