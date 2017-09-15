using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.UI.Navigation
{
    public interface IMenuProvider
    {
        void BuildMenu(MenuBuilder builder);
    }
}
