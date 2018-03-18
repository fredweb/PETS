using System.Collections.Generic;
using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class CategoriaRecord : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual string Sigla { get; set; }
        public virtual ICollection<MaterialRecord> Materiais {get;set;}
    }

    public class CategoriaMap : EntityMap<CategoriaRecord>
    {
        public CategoriaMap ( )
        {
            Table ( "CATEGORIA" );
            Id ( i => i.Id ).Column ( "SQCATEGORIA" ).GeneratedBy.Increment();
            Map ( m => m.Descricao ).Column ( "DSDESCRICAO" ).Length ( 500 ).Not.Nullable ( );
            Map ( m => m.Sigla ).Column ( "SGDESCRICAO" ).Length ( 5 ).Not.Nullable ( );
        }
    }
}