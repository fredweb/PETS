using System.Collections.Generic;
using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class SexoAnimalRecord
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public virtual ICollection<AnimalRecord> Animais { get; set; }
    }

    public class SexoAnimalMap : EntityMap<SexoAnimalRecord>
    {
        public SexoAnimalMap ( )
        {
            Table ( "SEXOANIMAL" );
            Id ( i => i.Id ).Column ( "IDANIMAL" ).GeneratedBy.Increment ( );
            Map ( m => m.Nome ).Column ( "NMSEXOANIMAL" ).Length ( 10 ).Not.Nullable ( );
            Map ( m => m.Sigla ).Column ( "SGSEXOANIMAL" ).Length ( 1 ).Not.Nullable ( );
        }
    }
}