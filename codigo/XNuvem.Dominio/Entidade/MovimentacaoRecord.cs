using XNuvem.Data;
using XNuvem.Dominio.Entidade.Base;

namespace XNuvem.Dominio.Entidade
{
    public class MovimentacaoRecord : BaseEntity
    {
        public virtual ServicoRecord Servico { get; set; }
        public virtual EstoqueRecord Estoque { get; set; }
        public virtual CaixaRecord Caixa { get; set; }
        public virtual long Quantidade { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual decimal Desconto { get; set; }
    }

    public class MovimentacaoMap : EntityMap<MovimentacaoRecord>
    {
        public MovimentacaoMap()
        {
            Table("MOVIMENTACAO");
            Id(k => k.Id).Column("SQMOVIMENTACAO").GeneratedBy.Increment();
            Map(m => m.Quantidade).Column("QTQUANTIDADE");
            Map(m => m.Valor).Column("VLVALOR").Scale(7).Precision(2).Not.Nullable();
            Map(m => m.Desconto).Column("VLDESCONTO").Scale(7).Precision(2);
            References(r => r.Servico).Column("SQSERVICO").ForeignKey().Cascade.None();
            References(r => r.Estoque).Column("SQESTOQUE").ForeignKey().Cascade.None();
            References(r => r.Caixa).Column("SQCAIXA").ForeignKey().Cascade.None();
        }
    }
}
