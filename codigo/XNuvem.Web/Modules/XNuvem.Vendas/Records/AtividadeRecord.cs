using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XNuvem.Data;

namespace XNuvem.Vendas.Records
{
    public class AtividadeRecord : UserIdentifiedRecord
    {
        public virtual int iAbsEntry { get; set; }
        public virtual string DocType { get; set; }
        public virtual int DocNum { get; set; }
        public virtual string CardCode { get; set; }
        public virtual int SlpCode { get; set; }
        public virtual string Comments { get; set; }
    }

    public class AtividadeRecordMap : EntityMap<AtividadeRecord>
    {
        public AtividadeRecordMap() {
            Table("Atividades");

            Id(m => m.iAbsEntry).GeneratedBy.Identity();

            Map(m => m.DocType).Length(20).Not.Nullable();
            Map(m => m.DocNum).Not.Nullable();
            Map(m => m.CardCode).Length(20).Not.Nullable();
            Map(m => m.SlpCode);
            Map(m => m.Comments).Length(2000).Nullable();

            this.MapUserIdentified();
        }
    }
}