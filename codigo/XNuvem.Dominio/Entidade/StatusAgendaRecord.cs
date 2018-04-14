using System.Collections.Generic;
using XNuvem.Data;
using XNuvem.Dominio.Entidade.Base;

namespace XNuvem.Dominio.Entidade
{
    public class StatusAgendaRecord : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public virtual ICollection<AgendaRecord> Agenda { get; set; }
    }

    public class StatusAgendaMap : EntityMap<StatusAgendaRecord>
    {
        public StatusAgendaMap()
        {
            Table("STATUSAGENDA");
            ReadOnly();
            Id(k => k.Id).Column("SQSTATUSAGENDA").GeneratedBy.Increment();
            Map(m => m.Nome).Column("NMSTATUS").Length(500).Not.Nullable();
            Map(m => m.Sigla).Column("SGSTATUS").Length(5).Not.Nullable();
        }
    }
}
