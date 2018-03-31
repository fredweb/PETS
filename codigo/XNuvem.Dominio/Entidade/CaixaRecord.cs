using System.Collections.Generic;
using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class CaixaRecord : BaseEntity
    {
        public virtual ClienteRecord Cliente { get; set; }
        public virtual FuncionarioRecord Funcionario { get; set; }
        public virtual ICollection<MovimentacaoRecord> Movimentacoes { get; set; }
        public virtual ICollection<PagamentoRecord> Pagamentos { get; set; }
    }

    public class CaixaMap : EntityMap<CaixaRecord>
    {
        public CaixaMap()
        {
            Table("CAIXA");
            Id(k => k.Id).Column("SQCAIXA").GeneratedBy.Increment();
            References(r => r.Cliente).Column("SQCLIENTE").ForeignKey().Not.Nullable();
            References(r => r.Funcionario).Column("SQFUNCIONARIO").ForeignKey().Not.Nullable();
        }
    }
}
