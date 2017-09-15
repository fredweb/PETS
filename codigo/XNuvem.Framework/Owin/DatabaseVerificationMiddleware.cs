using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XNuvem.Owin;
using Autofac;
using Autofac.Integration.Owin;
using XNuvem.Environment.Configuration;

namespace XNuvem.Owin
{
    public class DatabaseVerificationMiddleware : OwinMiddleware
    {
        public DatabaseVerificationMiddleware(OwinMiddleware next)
            : base(next) {

        }

        public async override Task Invoke(IOwinContext context) {
            if (context.Request.Method.Equals("GET", StringComparison.InvariantCultureIgnoreCase)) {
                bool redirectToInstall = false;
                if (!context.Request.Uri.AbsolutePath.Contains("/settings/install/install")) {
                    var scope = context.GetAutofacLifetimeScope();
                    var shellSettingsManager = scope.Resolve<IShellSettingsManager>();
                    // Verify if has configuration file and if the file contains connection string
                    // if not, redirect to install path
                    if (shellSettingsManager.HasConfigurationFile()) {
                        var settings = shellSettingsManager.GetSettings();
                        if (string.IsNullOrEmpty(settings.ConnectionSettings.DataConnectionString)) {
                            redirectToInstall = true;
                        }
                    }
                    else {
                        redirectToInstall = true;
                    }
                }
                if (redirectToInstall) {
                    context.Response.Redirect(new PathString("~/settings/install/install").Value);
                    return; // End response and not continue to next pipe
                }
            }
            await Next.Invoke(context);
        }
    }
}
