using System;
using XNuvem.Data;

namespace XNuvem.DDD.Entity
{
    public class ClienteRecord
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual DateTime DtNascimento { get; set; }
    }

    public class ClienteMap : EntityMap<ClienteRecord>
    {
        public ClienteMap()
        {
            Table("cliente");
            Id(w => w.Id).GeneratedBy.Identity();
            Map(w => w.Nome).Length(255).Not.Nullable();
            Map(w => w.DtNascimento).Not.Nullable();
        }
    }
}