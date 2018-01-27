using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class RacaRecord
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sigla  { get; set; }
    }

    public class RacaMap : EntityMap<RacaRecord>
    {
        public RacaMap()
        {
            Table("RACA");
            Id(i => i.Id).Column("IDRACA").GeneratedBy.Increment();
            Map(m => m.Nome).Column("NMRACA").Length(255).Not.Nullable();
            Map(m => m.Sigla).Column("SGRACA").Length(5).Not.Nullable();
        }
    }
}
