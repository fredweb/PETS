﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.UI.Messages
{
    public enum MessageType
    {
        Information,
        Success,
        Warning,
        Error
    }

    public interface IDisplayMessages : IDependency
    {
        void AddMessage(MessageType type, string message);
        IEnumerable<MessageEntry> GetMessages();
    }

    public class DisplayMessages : IDisplayMessages
    {
        private IList<MessageEntry> _entries;

        public DisplayMessages() {
            _entries = new List<MessageEntry>();
        }

        public void AddMessage(MessageType type, string message) {
            _entries.Add(new MessageEntry(type, message));
        }

        public IEnumerable<MessageEntry> GetMessages() {
            return _entries.ToArray();
        }
    }
}
