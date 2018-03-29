using System.Collections.Generic;
using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class TipoFuncionarioRecord : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual string Sigla { get; set; }
        public virtual ICollection<FuncionarioRecord> Funcionarios { get; set; }
    }

    public class TipoFuncinarioMap : EntityMap<TipoFuncionarioRecord>
    {
        public TipoFuncinarioMap()
        {
            Table("TIPOFUNCIONARIO");
            Id(k => k.Id).Column("SQTIPOFUNCIONARIO").GeneratedBy.Increment();
            Map(m => m.Descricao).Column("DSTIPOFUNCIONARIO").Length(500).Not.Nullable();
            Map(m => m.Sigla).Column("SGTIPOFUNCIONARIO").Length(5).Unique().Not.Nullable();
        }
    }
}
