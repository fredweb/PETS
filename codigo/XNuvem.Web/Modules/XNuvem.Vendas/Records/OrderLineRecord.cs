using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using XNuvem.Data;

namespace XNuvem.Vendas.Records
{
    public class OrderLineRecord
    {
        public int iAbsEntry { get; set; }
        [ScriptIgnore]
        public OrderRecord Order { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
    }

    public class OrderLineRecordMap: EntityMap<OrderLineRecord>
    {
        public OrderLineRecordMap() {
            Table("OrderLines");
            
            Id(m => m.iAbsEntry).GeneratedBy.Identity();
            
            References(m => m.Order).Column("iDocEntry").Not.Nullable();

            Map(m => m.ItemCode).Length(20).Not.Nullable();
            Map(m => m.ItemName).Length(100).Not.Nullable();
            Map(m => m.Quantity).Not.Nullable();
            Map(m => m.Price).Not.Nullable();
            Map(m => m.LineTotal).Not.Nullable();
        }
    }
}