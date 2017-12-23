using XNuvem.Data;

namespace XNuvem.DDD.Entity
{
    public class CategoriaRecord
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
    }

    public class CategoriaMap : EntityMap<CategoriaRecord>
    {
        public CategoriaMap()
        {
            Table("Categoria");
            Id(w => w.Id).GeneratedBy.Identity();
            Map(w => w.Nome).Length(255).Not.Nullable();
            Map(w => w.Sigla).Length(3).Not.Nullable();
        }
    }
}