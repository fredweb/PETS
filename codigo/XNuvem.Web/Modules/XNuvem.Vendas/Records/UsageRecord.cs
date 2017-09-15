using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XNuvem.Data;

namespace XNuvem.Vendas.Records
{
    public class UsageRecord
    {
        public int AbsEntry { get; set; }
        public string Name { get; set; }
        public int Usage { get; set; }
        public string UsageText { get; set; }
    }

    public class UsageRecordMap : EntityMap<UsageRecord>
    {
        public UsageRecordMap() {
            Table("Usages");
            Id(x => x.AbsEntry).GeneratedBy.Identity();
            Map(x => x.Name).Length(100).Not.Nullable();
            Map(x => x.Usage).Not.Nullable();
            Map(x => x.UsageText).Length(100).Nullable();
        }
    }
}