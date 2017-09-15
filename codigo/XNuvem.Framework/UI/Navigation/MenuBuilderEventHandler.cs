using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNuvem.Environment;
using XNuvem.Logging;

namespace XNuvem.UI.Navigation
{
    public class MenuBuilderEventHandler : IHostEvents
    {
        private readonly IMenuManager _menuManager;
        private readonly IEnumerable<IMenuProvider> _providers;


        public ILogger Logger { get; set; }

        public MenuBuilderEventHandler(IMenuManager menuManager, IEnumerable<IMenuProvider> providers) {
            _menuManager = menuManager;
            _providers = providers;
            Logger = NullLogger.Instance;
        }

        public void OnInitialize() {
            _menuManager.BuildMenu(_providers);
        }

        public void OnTerminate() {
            
        }
    }
}
