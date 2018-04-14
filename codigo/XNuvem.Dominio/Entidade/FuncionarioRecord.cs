using System;
using System.Collections.Generic;
using XNuvem.Data;
using XNuvem.Dominio.Entidade.Base;

namespace XNuvem.Dominio.Entidade
{
    public class FuncionarioRecord : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual string Telefone { get; set; }
        public virtual TipoFuncionarioRecord TipoFuncionario { get; set; }
        public virtual SexoRecord Sexo { get; set; }
        public virtual ICollection<AgendaRecord> Agenda { get; set; }
        public virtual ICollection<CaixaRecord> Caixa { get; set; }
    }

    public class FuncionarioMap : EntityMap<FuncionarioRecord>
    {
        public FuncionarioMap()
        {
            Table("FUNCIONARIO");
            Id(k => k.Id).Column("SQFUNCIONARIO").GeneratedBy.Increment();
            Map(m => m.Nome).Column("NMNOME").Length(500).Not.Nullable();
            Map(m => m.Telefone).Column("DSTELEFONE").Length(20);
            Map(m => m.DataNascimento).Column("DTNASCIMENTO").Not.Nullable();
            References(r => r.Sexo).Column("SQSEXO").Cascade.None().Not.Nullable();
            References(r => r.TipoFuncionario).Column("SQTIPOFUNCIONARIO").Cascade.None().Not.Nullable();
        }
    }
}
