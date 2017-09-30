using XNuvem.Data;

namespace XNuvem.DDD.Entity
{
    public class RacaRecord
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public virtual EspecieRecord Especie { get; set; }
    }

    public class RacaMap : EntityMap<RacaRecord>
    {
        public RacaMap()
        {
            Table("Raca");
            Id(w => w.Id).GeneratedBy.Identity();
            Map(w => w.Nome).Length(100).Not.Nullable();
            Map(w => w.Sigla).Length(3).Not.Nullable();
            References(w => w.Especie).ForeignKey("FKESPECIERACA").Column("ESPECIEID");
        }
    }

}
