using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class ServicoRecord : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public virtual decimal Valor { get; set; }
    }

    public class ServicoMap : EntityMap<ServicoRecord>
    {
        public ServicoMap()
        {
            Table("SERVICO");
            Id(k => k.Id).Column("SQSERVIVO").GeneratedBy.Increment();
            Map(m => m.Nome).Column("NMSERVICO").Length(500).Not.Nullable();
            Map(m => m.Sigla).Column("SGSERVICO").Length(5).Not.Nullable();
        }
    }
}
