using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.UI
{
    public class XNuvemUIException : XNuvemCoreException
    {
        public XNuvemUIException(string message)
            : base(message) 
        {

        }

        public XNuvemUIException(string message, Exception innerException)
            : base(message, innerException) 
        {

        }

    }
}
