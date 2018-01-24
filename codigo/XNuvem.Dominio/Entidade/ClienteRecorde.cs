using System;
using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class ClienteRecord
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual DateTime DtNascimento { get; set; }
        public virtual long SexoId { get; set; }
    }

    public class ClienteMap : EntityMap<ClienteRecord>
    {
        public ClienteMap()
        {
            Table("Cliente");
            Id(w => w.Id).Column("SQCLIENTE").GeneratedBy.Identity();
            Map(m => m.Nome).Column("MNNOME").Length(500).Not.Nullable();
            Map(m => m.DtNascimento).Column("DTNASCIMENTO").Not.Nullable();
            Map(m => m.SexoId).Column("SQSEXO").Not.Nullable();
        }
    }
}