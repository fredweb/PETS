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
