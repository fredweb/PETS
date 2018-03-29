using System.Collections.Generic;
using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class FornecedorRecord : BaseEntity
    {
        public virtual string Fantasia { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual string Telefone { get; set; }
        public virtual ICollection<EstoqueRecord> Estoque { get; set; }
    }
    public class FornecedorMap: EntityMap<FornecedorRecord>
    {
        public FornecedorMap()
        {
            Table("FORNECEDOR");
            Id(k => k.Id).Column("SQFORNECEDOR").GeneratedBy.Increment();
            Map(m => m.RazaoSocial).Column("NMRAZAOSOCIAL").Length(500).Not.Nullable();
            Map(m => m.Fantasia).Column("NMFANTASIA").Length(500).Not.Nullable();
            Map(m => m.Telefone).Column("NUTELEFONE").Length(20).Not.Nullable();
        }
    }
}
