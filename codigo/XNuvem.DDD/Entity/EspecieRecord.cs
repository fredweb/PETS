using XNuvem.Data;

namespace XNuvem.DDD.Entity
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
            Table("Especie");
            Id(w => w.Id).GeneratedBy.Identity();
            Map(w => w.Nome).Length(255).Not.Nullable();
            Map(w => w.Sigla).Length(3).Not.Nullable();
        }
    }
}
