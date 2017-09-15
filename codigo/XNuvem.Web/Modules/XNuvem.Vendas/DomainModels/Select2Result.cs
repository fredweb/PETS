using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XNuvem.Vendas.DomainModels
{
    public class Select2Result<TResult> where TResult : class
    {
        public int start { get; set; }
        public int length { get; set; }
        public int total { get; set; }
        public string q { get; set; }
        public IEnumerable<TResult> results { get; set; }
    }
}