using XNuvem.Data;
using XNuvem.Dominio.Entidade.Base;

namespace XNuvem.Dominio.Entidade
{
    public class PagamentoRecord : BaseEntity
    {
        public virtual decimal ValorTotal { get; set; }
        public virtual decimal Desconto { get; set; }
        public virtual CaixaRecord Caixa { get; set; }
        public virtual FormaPagamentoRecord FormaPagamento { get; set; }
    }
    public class PagamentoMap: EntityMap<PagamentoRecord>
    {
        public PagamentoMap()
        {
            Table("PAGAMENTO");
            Id(k => k.Id).Column("SQPAGAMENTO").GeneratedBy.Increment();
            Map(m => m.ValorTotal).Column("VLVALORTOTAL").Scale(2).Precision(7).Not.Nullable();
            Map(m => m.Desconto).Column("NUDESCONTO").Scale(2).Precision(7).Not.Nullable();
            References(r => r.Caixa).Column("SQCAIXA").ForeignKey().Not.Nullable();
            References(r => r.FormaPagamento).Column("SQFORMAPAGAMENTO").ForeignKey().Not.Nullable();
        }
    }
}
