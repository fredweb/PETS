using System;
using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class AnimalRecord
    {
        public virtual long Id { get; set; }
        public virtual long  SexoId { get; set; }
        public virtual long  RacaId { get; set; }
        public virtual string  Nome { get; set; }
        public virtual DateTime DataNascimento { get; set; }

        public virtual SexoAnimalRecord Sexo { get; set; }
        public virtual RacaRecord Raca { get; set; }
    }

    public class AnimalMap : EntityMap<AnimalRecord>
    {
        public AnimalMap()
        {
            Table("Animal");
            Id(i => i.Id).Column("IDANIMAL").GeneratedBy.Increment();
            Map(m => m.Nome).Column("NMNOME").Length(255).Not.Nullable();
            Map(m => m.DataNascimento).Column("DTNASCIMENTO").Not.Nullable();

        }
    }
}
