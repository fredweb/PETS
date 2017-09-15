using Autofac;
using Autofac.Integration.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XNuvem.Data;
using XNuvem.Environment;
using XNuvem.Logging;
using XNuvem.UI.Messages;

namespace XNuvem.Exceptions.Filters
{
    public class UnhandledExceptionFilter : HandleErrorAttribute
    {   
        public UnhandledExceptionFilter() {
            
        }

        public override void OnException(ExceptionContext filterContext) {
            if (!filterContext.ExceptionHandled && filterContext.Exception != null) {
                IPolicyException policyException = null;
                if (XNuvemServices.Current.TryResolve<IPolicyException>(out policyException)) {
                    filterContext.ExceptionHandled = policyException.HandleException(filterContext.Exception);

                    if (filterContext.ExceptionHandled) {
                        // Se for uma chamada em Ajax, retorna em json o resultado do erro.
                        if (string.Compare(filterContext.HttpContext.Request.Headers["X-Requested-With"], "XMLHttpRequest", true) == 0) {
                            filterContext.HttpContext.Response.Clear();
                            //filterContext.Result = new ErrorJsonResult(filterContext.Exception);
                            filterContext.Result = new ContentResult {
                                Content = filterContext.Exception.Message,
                                ContentType = "text/plain"
                            };
                        }
                        else {
                            // Caso contrário mostra a página de erro padrão.
                            filterContext.Result = new ViewResult() {
                                ViewData = filterContext.Controller.ViewData,
                                TempData = filterContext.Controller.TempData,
                                ViewName = "ErrorPage"
                            };
                        }
                        filterContext.RequestContext.HttpContext.Response.StatusCode = 500;
                        // prevent IIS 7.0 classic mode from handling the 404/500 itself
                        filterContext.RequestContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    }
                }
            }
            base.OnException(filterContext);
        }
    }
}
