using XNuvem.Data;
using XNuvem.Dominio.Entidade.Base;

namespace XNuvem.Dominio.Entidade
{
    public class DocumentoRecord : BaseEntity
    {
        public virtual TipoDocumentoRecord TipoDocumento { get; set; }
        public virtual FuncionarioRecord Funcionario { get; set; }
        public virtual string Valor { get; set; }
    }

    public class DocumentoMap : EntityMap<DocumentoRecord>
    {
        public DocumentoMap()
        {
            Table("TIPODOCUMENTOFUNCIONARIO");
            Id(i => i.Id).Column("SQDOCUMENTO").GeneratedBy.Increment().Not.Nullable();
            References(r => r.Funcionario).Column("SQFUNCIONARIO").UniqueKey("SQFUNCIONARIODOCUMENTO");
            References(r => r.TipoDocumento).Column("SQTIPODOCUMENTO").UniqueKey("SQFUNCIONARIODOCUMENTO");
            Map(m => m.Valor).Column("VLVALOR").Length(500).Not.Nullable();
        }
    }
}
