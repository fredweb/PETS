using System.Collections.Generic;
using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class SexoRecord
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public virtual ICollection<FuncionarioRecord> Funcionarios { get; set; }
    }

    public class SexoMap : EntityMap<SexoRecord>
    {
        public SexoMap ( )
        {
            Table ( "SEXO" );
            Id ( i => i.Id ).Column ( "IDSEXO" ).GeneratedBy.Increment ( );
            Map ( m => m.Nome ).Column ( "NMNOME" ).Length ( 20 ).Not.Nullable ( );
            Map ( m => m.Sigla ).Column ( "SGSEXO" ).Length ( 1 ).Not.Nullable ( );
        }
    }
}