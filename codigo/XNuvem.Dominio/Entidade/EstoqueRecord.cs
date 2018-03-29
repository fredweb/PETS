using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class EstoqueRecord : BaseEntity
    {
        public virtual string Remessa { get; set; }
        public virtual string Lote { get; set; }
        public virtual long Quantidade { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual FornecedorRecord Fornecedor { get; set; }
        public virtual MaterialRecord Material { get; set; }
    }
    public class EstoqueMap : EntityMap<EstoqueRecord>
    {
        public EstoqueMap()
        {
            Table("ESTOQUE");
            Id(k => k.Id).Column("SQESTOQUE").GeneratedBy.Increment();
            Map(m => m.Lote).Column("NULOTE").Length(100).Not.Nullable();
            Map(m => m.Remessa).Column("NUREMESSA").Length(100).Not.Nullable();
            Map(m => m.Quantidade).Column("NUQUANTIDADE").Not.Nullable();
            Map(m => m.Valor).Column("NUVALORUNITARIO").Scale(7).Precision(2).Not.Nullable();
            References(r => r.Fornecedor).Column("SQFORNECEDOR").Cascade.None().Not.Nullable();
            References(r => r.Material).Column("SQMATERIAL").Cascade.None().Not.Nullable();
        }
    }
}
