using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XNuvem.UI.Messages;
using System.Web.Mvc.Html;

namespace XNuvem.Mvc.Html
{
    public static class DisplayExtensions
    {
        public static MvcHtmlString DisplayErrors(this HtmlHelper html, IEnumerable<MessageEntry> messages) {
            //if (messages.Any()) {
            //    var errors = messages.Where(m => m.Type == MessageType.Error).ToList();
            //}
            return MvcHtmlString.Empty;
        }
    }
}
