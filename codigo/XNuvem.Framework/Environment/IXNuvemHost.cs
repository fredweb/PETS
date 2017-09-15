using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Environment
{
    public interface IXNuvemHost
    {
        void OnStart();

        void OnTerminate();
    }
}
