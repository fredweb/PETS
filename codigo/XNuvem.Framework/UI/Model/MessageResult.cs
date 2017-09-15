using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.UI.Model
{
    public class MessageResult
    {
        public bool IsError { get; set; }

        public IEnumerable<string> Messages { get; set; }
    }
}
