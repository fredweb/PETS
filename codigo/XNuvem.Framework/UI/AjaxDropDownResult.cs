using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.UI
{
    public class AjaxDropDownResultItem
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class AjaxDropDownResult
    {
        public IList<AjaxDropDownResultItem> results { get; set; }
    }
}
