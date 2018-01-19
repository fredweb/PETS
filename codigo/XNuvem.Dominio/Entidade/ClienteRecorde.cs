using System;
using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class ClienteRecord
    {
        public string Nome { get; set; }
        public DateTime DtNascimento { get; set; }
        public long SexoId { get; set; }

    }

    public class ClienteMap : EntityMap<ClienteRecord>
    {
        public ClienteMap()
        {
                Table("Cliente");
        }
    }
}
