using System;
using System.Collections.Generic;
using XNuvem.Data;
using XNuvem.Dominio.Entidade.Base;

namespace XNuvem.Dominio.Entidade
{
    public class ClienteRecord : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual DateTime DtNascimento { get; set; }
        public virtual long SexoId { get; set; }
        public virtual ICollection<AnimalRecord> Animais { get; set; }
        public virtual ICollection<AgendaRecord> Agenda { get; set; }
        public virtual ICollection<CaixaRecord> Caixa { get; set; }
    }

    public class ClienteMap : EntityMap<ClienteRecord>
    {
        public ClienteMap()
        {
            Table("CLIENTE");
            Id(w => w.Id).Column("IDCLIENTE").GeneratedBy.Increment();
            Map(m => m.Nome).Column("MNNOME").Length(500).Not.Nullable();
            Map(m => m.DtNascimento).Column("DTNASCIMENTO").Not.Nullable();
            Map(m => m.SexoId).Column("IDSEXO").Not.Nullable();
        }
    }
}