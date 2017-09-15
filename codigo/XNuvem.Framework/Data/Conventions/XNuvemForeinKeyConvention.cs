using FluentNHibernate.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Data.Conventions
{
    public class XNuvemForeinKeyConvention : ForeignKeyConvention
    {       
        protected override string GetKeyName(FluentNHibernate.Member property, Type type) {
            if (property == null) {
                return type.Name + "Code";
            }

            if (property.PropertyType == typeof(Int32) || property.PropertyType == typeof(Int64)) {
                return type.Name + "Entry";
            }

            return type.Name + "Code";
        }
    }
}
