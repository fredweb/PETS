using System.Collections.Generic;
using XNuvem.Data;
using XNuvem.Dominio.Entidade.Base;

namespace XNuvem.Dominio.Entidade
{
    public class FormaPagamentoRecord : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public virtual ICollection<PagamentoRecord> Pagamentos { get; set; }
    }

    public class FormaPagamentoMap : EntityMap<FormaPagamentoRecord>
    {
        public FormaPagamentoMap()
        {
            Table("FORMAPAGAMENTO");
            ReadOnly();
            Id(k => k.Id).Column("SQFORMAPAGAMENTO").GeneratedBy.Increment();
            Map(m => m.Nome).Column("NMFORMAPAGAMENTO").Length(500).Not.Nullable();
            Map(m => m.Sigla).Column("SGFORMAPAGAMENTO").Length(5).Not.Nullable();
        }
    }
}
