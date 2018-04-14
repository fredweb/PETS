using System;
using XNuvem.Data;
using XNuvem.Dominio.Entidade.Base;

namespace XNuvem.Dominio.Entidade
{
    public class AnimalRecord : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual SexoAnimalRecord Sexo { get; set; }
        public virtual RacaRecord Raca { get; set; }
        public virtual ClienteRecord Dono { get; set; }
    }

    public class AnimalMap : EntityMap<AnimalRecord>
    {
        public AnimalMap()
        {
            Table("ANIMAL");
            Id(i => i.Id).Column("SQANIMAL").GeneratedBy.Increment();
            Map(m => m.Nome).Column("NMNOME").Length(255).Not.Nullable();
            Map(m => m.DataNascimento).Column("DTNASCIMENTO").Not.Nullable();
            References(r => r.Sexo).Column("SQSEXO").Not.Nullable().Cascade.None();
            References(r => r.Raca).Column("SQRACA").Not.Nullable().Cascade.None();
            References(r => r.Dono).Column("SQCLIENTE").Not.Nullable().Cascade.None();

        }
    }
}