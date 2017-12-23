using XNuvem.Data;

namespace XNuvem.DDD.Entity
{
    public class MaterialRecord
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        public virtual CategoriaRecord Categoria { get; set; }
    }

    public class MaterialMap : EntityMap<MaterialRecord>
    {
        public MaterialMap()
        {
            Table("material");
            Id(w => w.Id).GeneratedBy.Identity();
            Map(w => w.Nome).Length(255).Not.Nullable();
            Map(w => w.Descricao).Length(1000).Not.Nullable();
            References(w => w.Categoria).ForeignKey("FKCATEGORIAMATERIAL").Column("CATEGORIAID");
        }
    }
}