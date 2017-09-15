using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace XNuvem.UI.Messages
{
    public class DisplayMessagesJsonResult : JsonResult
    {
        public DisplayMessagesJsonResult(IEnumerable<MessageEntry> messages) {
            Data = new { @IsError = true, @Messages = messages };
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }
    }
}
