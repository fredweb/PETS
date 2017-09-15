using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using XNuvem.Logging;

namespace XNuvem.Environment
{
    public class DefaultShellEventHandler : IShellEvents
    {
        public ILogger Logger { get; set; }

        public DefaultShellEventHandler() {
            Logger = NullLogger.Instance;
        }

        public void OnBeginRequest(IOwinContext context) {
            Logger.Debug("On begin request - " + context.Request.Uri.ToString());
        }

        public void OnEndRequest(IOwinContext context) {
            Logger.Debug("On end request - " + context.Request.Uri.ToString());
        }

        public void OnInitialize() {
            Logger.Debug("On application initialize");
        }

        public void OnTerminate() {
            Logger.Debug("On application terminate");
        }
    }
}
