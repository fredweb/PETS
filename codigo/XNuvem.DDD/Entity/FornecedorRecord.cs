using XNuvem.Data;

namespace XNuvem.DDD.Entity
{
    public class FornecedorRecord
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Razao { get; set; }
        public virtual string Telefone { get; set; }
    }

    public class FornecedorMap : EntityMap<FornecedorRecord>
    {
        public FornecedorMap()
        {
            Table("Fornecedor");
            Id(w => w.Id).GeneratedBy.Identity();
            Map(w => w.Nome).Length(255).Not.Nullable();
            Map(w => w.Razao).Length(255).Not.Nullable();
            Map(w => w.Telefone).Length(14).Not.Nullable();
        }
    }
}
