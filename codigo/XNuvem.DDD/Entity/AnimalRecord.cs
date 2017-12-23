using System;
using XNuvem.Data;

namespace XNuvem.DDD.Entity
{
    public class AnimalRecord
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual DateTime Dtnascimento { get; set; }
        public virtual RacaRecord Raca { get; set; }
    }

    public class AnimalMap : EntityMap<AnimalRecord>
    {
        public AnimalMap()
        {
            Table("animal");
            Id(w => w.Id).GeneratedBy.Identity();
            Map(w => w.Nome).Length(500).Not.Nullable();
            Map(w => w.Dtnascimento).Not.Nullable();
            References(w => w.Raca).ForeignKey("FKRACAANIMAL").Column("RacaId");
        }
    }
}