using System;

namespace ReportViewBase.Base.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UriParameterAttribute : Attribute
    {
        public UriParameterAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}