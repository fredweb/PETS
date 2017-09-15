using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XNuvem.Data;

namespace XNuvem.Vendas.Records
{
    public class OrderRecord : UserIdentifiedRecord
    {
        public OrderRecord() {
            DocStatus = "W";
            Canceled = "N";
        }
        public virtual int iDocEntry { get; set; }
        public virtual int SlpCode { get; set; }
        public virtual DateTime DocDate { get; set; }
        public virtual string CardCode { get; set; }
        public virtual string CardName { get; set; }
        public virtual string RotaCode { get; set; }
        public virtual string RotaName { get; set; }
        public virtual int GroupNum { get; set; }
        public virtual string PymntGroup { get; set; }
        public virtual string PeyMethod { get; set; }
        public virtual int ListNum { get; set; }
        public virtual string ListName { get; set; }
        public virtual string Comments { get; set; }
        public virtual bool InSbo { get; set; }
        public virtual int DocEntry { get; set; }
        public virtual bool Approved { get; set; }
        public virtual string Canceled { get; set; }
        public virtual string DocStatus { get; set; }
        public virtual double DocTotal { get; set; }
        public IList<OrderLineRecord> Lines { get; set; }
        public virtual int Usage { get; set; }
        public virtual string Carga { get; set; }
    }

    public class OrderRecordMap : EntityMap<OrderRecord>
    {
        public OrderRecordMap() {
            Table("Orders");

            Id(m => m.iDocEntry).GeneratedBy.Identity();

            Map(m => m.SlpCode).Not.Nullable().Index("IX_Orders_SlpCode");
            Map(m => m.DocDate).Not.Nullable();
            Map(m => m.CardCode).Length(20).Not.Nullable();
            Map(m => m.CardName).Length(100);
            Map(m => m.RotaCode).Length(30);
            Map(m => m.RotaName).Length(255);
            Map(m => m.GroupNum);
            Map(m => m.PymntGroup).Length(100);
            Map(m => m.PeyMethod).Length(15);
            Map(m => m.ListNum).Not.Nullable();
            Map(m => m.ListName).Length(100);
            Map(m => m.Comments).Length(500).Nullable();
            Map(m => m.InSbo).Not.Nullable();
            Map(m => m.DocEntry).Not.Nullable().Index("IX_Orders_DocEntry");
            Map(m => m.Approved).Not.Nullable();
            Map(m => m.Canceled).Length(1).Default("('N')").Not.Nullable();
            Map(m => m.DocStatus).Length(1).Default("('W')").Not.Nullable();
            Map(m => m.DocTotal).Not.Nullable();
            Map(m => m.Usage).Not.Nullable().Default("(-1)");

            HasMany(m => m.Lines).Inverse().Cascade.All().KeyColumn("iDocEntry");

            this.MapUserIdentified();
            Map(m => m.Carga).Length(10);
        }
    }
}