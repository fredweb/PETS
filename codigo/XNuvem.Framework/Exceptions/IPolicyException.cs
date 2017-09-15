using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Exceptions
{
    public interface IPolicyException
    {
        bool HandleException(Exception ex);
    }
}
