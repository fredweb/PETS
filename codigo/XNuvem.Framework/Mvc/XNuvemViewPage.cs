using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XNuvem.Environment;
using XNuvem.UI.Messages;

namespace XNuvem.Mvc
{
    public class XNuvemViewPage<TModel> : WebViewPage<TModel>
    {
        public IServiceContext ServiceContext { get; set; }

        public override void InitHelpers() {
            base.InitHelpers();
            ServiceContext = XNuvemServices.Current;
        }

        public override void Execute() {

        }
    }

    public class XNuvemViewPage : ViewPage<dynamic>
    {

    }
}
