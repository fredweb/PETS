using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XNuvem.Data;

namespace XNuvem.Vendas.Records
{
    public class PriceSlpRecord : UserIdentifiedRecord
    {
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
        public double PriceDsc { get; set; }

        public override bool Equals(object obj) {
            var other = obj as PriceSlpRecord;
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.SlpCode == other.SlpCode && this.ItemCode == other.ItemCode;
        }

        public override int GetHashCode() {
            unchecked {
                int hash = GetType().GetHashCode();
                hash = (hash * 37) ^ SlpCode.GetHashCode();
                hash = (hash * 37) ^ ItemCode.GetHashCode();
                return hash;
            }
        }
    }

    public class PriceSlpRecordMap : EntityMap<PriceSlpRecord>
    {
        public PriceSlpRecordMap() {
            Table("PriceSlp");
            CompositeId()
                .KeyProperty(x => x.SlpCode)
                .KeyProperty(x => x.ItemCode, kp => kp.Length(20));
            Map(x => x.SlpName).Length(100);
            Map(x => x.ItemName).Length(100);
            Map(x => x.Price).CustomSqlType("NUMERIC(18,6)").Not.Nullable();
            Map(x => x.PriceDsc).CustomSqlType("NUMERIC(18,6)").Not.Nullable();
        }
    }
}