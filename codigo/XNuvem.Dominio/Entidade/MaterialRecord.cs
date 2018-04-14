using System.Collections.Generic;
using XNuvem.Data;
using XNuvem.Dominio.Entidade.Base;

namespace XNuvem.Dominio.Entidade
{
    public class MaterialRecord : BaseEntity
    {
        public virtual CategoriaRecord Categoria { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        public virtual ICollection<EstoqueRecord> Estoque { get; set; }
    }

    public class MaretialMap : EntityMap<MaterialRecord>
    {
        public MaretialMap ( )
        {
            Table ( "MATERIAL" );
            Id ( i => i.Id ).Column ( "SQMATERIAL" ).GeneratedBy.Increment ( );
            Map ( m => m.Nome ).Column ( "NMNOME" ).Length ( 255 ).Not.Nullable ( );
            Map ( m => m.Descricao ).Column ( "DSMATERIAL" ).Length ( 500 ).Not.Nullable ( );
            References ( r => r.Categoria ).Column ( "SQCATEGORIA" ).Cascade.None ( ).Not.Nullable();
        }
    }
}
