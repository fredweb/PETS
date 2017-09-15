using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNuvem.Environment.Extensions;

namespace XNuvem.Environment
{
    public static class PreApplicationStarter
    {
        public static void Start() {
            ExtensionLoader.Load();
        }
    }
}
