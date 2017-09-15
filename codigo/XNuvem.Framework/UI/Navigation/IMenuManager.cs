using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.UI.Navigation
{
    public interface IMenuManager
    {
        void BuildMenu(IEnumerable<IMenuProvider> providers);
        IEnumerable<MenuEntry> GetMenuAsList(bool includeRoot);
        MenuEntry GetRootMenu();
    }
}
