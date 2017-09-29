/****************************************************************************************
 *
 * Autor: George Santos
 * Copyright (c) 2016  
 *
 * 
/****************************************************************************************/


using System;
using System.Runtime.Serialization;
using XNuvem.Localization;

namespace XNuvem
{
    [Serializable]
    public class XNuvemCoreException : Exception
    {
        private readonly LocalizedString _localizedMessage;

        public XNuvemCoreException(string message)
            : base(message) {

        }

        public XNuvemCoreException(string message, Exception innerException)
            : base(message, innerException) {

        }

        public XNuvemCoreException(LocalizedString message)
            : base(message.Text) {
            _localizedMessage = message;
        }

        public XNuvemCoreException(LocalizedString message, Exception innerException)
            : base(message.Text, innerException) {
            _localizedMessage = message;
        }

        protected XNuvemCoreException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }

        public LocalizedString LocalizedMessage { get { return _localizedMessage; } }
    }
}
