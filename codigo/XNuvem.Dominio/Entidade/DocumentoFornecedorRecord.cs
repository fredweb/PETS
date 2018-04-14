using XNuvem.Data;

namespace XNuvem.Dominio.Entidade
{
    public class DocumentoFornecedorRecord : BaseEntity
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
            Id(i => i.Id).Column("SQDOCUMENTOFORNECEDOR").GeneratedBy.Increment().Not.Nullable();
            Map(m => m.Valor).Column("VLVALOR").Length(500).Not.Nullable();
            References(r => r.Fornecedor).Column("SQFORNECEDOR").UniqueKey("KYFORNCEDORDOCUEMNTO").Not.Nullable();
            References(r => r.TipoDocumento).Column("SQTIPODOCUMENTO").UniqueKey("KYFORNCEDORDOCUEMNTO").Not.Nullable();
        }
    }
}
