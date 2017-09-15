using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XNuvem.Environment
{
    public class XNuvemServices : IServiceContext
    {
        public static IServiceContext Current {
            get {
                return new XNuvemServices(HttpContext.Current.GetOwinContext());
            }
        }

        private readonly IOwinContext _owinContext;
        public XNuvemServices(IOwinContext context) {
            _owinContext = context;
        }

        private ILifetimeScope Services {
            get { return _owinContext.GetAutofacLifetimeScope(); }
        }
        public TService Resolve<TService>() {
            return Services.Resolve<TService>();
        }

        public bool TryResolve<TService>(out TService service) {
            service = default(TService);
            try {
                service = Services.Resolve<TService>();
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
