using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class DocumentoFornecedorRecord
    {
        public virtual FornecedorRecord Fornecedor { get; set; }
        public virtual TipoDocumentoRecord TipoDocumento { get; set; }
        public virtual string Valor { get; set; }
    }

    public class DocumentoFornecedorMap : EntityMap<DocumentoFornecedorRecord>
    {
        public DocumentoFornecedorMap()
        {
            Table("TIPODOCUMENTOFORNECEDOR");
            References(r => r.Fornecedor).Column("SQFORNECEDOR").UniqueKey("KYFORNCEDORDOCUEMNTO").Not.Nullable();
            References(r => r.TipoDocumento).Column("SQTIPODOCUMENTO").UniqueKey("KYFORNCEDORDOCUEMNTO").Not.Nullable();
        }
    }
}
