using System;
using XNuvem.Data;
using XNuvem.Dominio.Entidade.Base;

namespace XNuvem.Dominio.Entidade
{
    public class AtendimentoRecord : BaseEntity
    {
        public virtual string Observacao { get; set; }
        public virtual DateTime DataInicialAtendimento { get; set; }
        public virtual DateTime DataFinalatendimento { get; set; }
        public virtual AgendaRecord Agenda { get; set; }
    }

    public class AtentimentoMap : EntityMap<AtendimentoRecord>
    {
        public AtentimentoMap()
        {
            Table("ATENDIMENTO");
            Id(k => k.Id).Column("SQATENDIMENTO").GeneratedBy.Increment();
            Map(m => m.Observacao).Column("DSOBSERVAOCAO").Length(4000);
            Map(m => m.DataInicialAtendimento).Column("DTINICIAL").Not.Nullable();
            Map(m => m.DataFinalatendimento).Column("DTFINAL").Not.Nullable();
            References(r => r.Agenda).Column("SQAGENDA").ForeignKey().Not.Nullable();
        }
    }
}
