using System;
using System.Collections.Generic;
using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class AgendaRecord : BaseEntity
    {
        public virtual ClienteRecord Cliente { get; set; }
        public virtual AnimalRecord Animal { get; set; }
        public virtual StatusAgendaRecord Status { get; set; }
        public virtual FuncionarioRecord Funcionario { get; set; }
        public virtual DateTime DataAgendamento { get; set; }
        public virtual DateTime DataAtendimento { get; set; }
        public virtual ICollection<AtendimentoRecord> Atendimentos { get; set; }
    }

    public class AgendaMap: EntityMap<AgendaRecord>
    {
        public AgendaMap()
        {
            Table("AGENDA");
            Id(k => k.Id).Column("SQAGENDA").GeneratedBy.Increment();
            Map(m => m.DataAgendamento).Column("DTAGENDAMENTO").Not.Nullable();
            Map(m => m.DataAtendimento).Column("DTATENDIMENTO").Not.Nullable();
            References(r => r.Animal).Column("SQANIMAL").ForeignKey("FKANIMALAGENDA").Not.Nullable();
            References(r => r.Status).Column("SQSTATUS").ForeignKey("FKSTATUSAGENDA").Not.Nullable();
            References(r => r.Funcionario).Column("SQFUNCIONARIO").ForeignKey("FKFUNCIONARIOAGENDA").Not.Nullable();
        }
    }
}
