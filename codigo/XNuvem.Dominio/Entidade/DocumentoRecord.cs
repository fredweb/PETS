using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class DocumentoRecord
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
            References(r => r.Funcionario).Column("SQFUNCIONARIO").UniqueKey("SQFUNCIONARIODOCUMENTO");
            References(r => r.TipoDocumento).Column("SQTIPODOCUMENTO").UniqueKey("SQFUNCIONARIODOCUMENTO");
            Map(m => m.Valor).Column("VLVALOR").Length(500).Not.Nullable();
        }
    }
}
