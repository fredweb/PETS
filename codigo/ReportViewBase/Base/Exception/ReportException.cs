using System;

namespace ReportViewBase.Base.Exception
{
    [Serializable]
    public class ReportException : System.Exception
    {
        public ReportException(string message)
            : base(message)
        {
        }

        public ReportException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}