using System;

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
