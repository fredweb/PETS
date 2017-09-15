using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Mvc
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AdminAttribute : Attribute
    {
        public string AreaName { get; protected set; }
        public string GroupName { get; protected set; }
        public AdminAttribute(string areaName, string groupName) {
            this.AreaName = areaName;
            this.GroupName = groupName;
        }
    }
}
