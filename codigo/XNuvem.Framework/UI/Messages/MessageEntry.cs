using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.UI.Messages
{
    public class MessageEntry
    {
        public MessageType Type { get; set; }
        public string Message { get; set; }

        public MessageEntry(MessageType type, string message) {
            this.Type = type;
            this.Message = message;
        }
    }
}
