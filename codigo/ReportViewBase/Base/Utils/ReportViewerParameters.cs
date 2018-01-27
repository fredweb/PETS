using System;

namespace ReportViewBase.Base.Utils
{
    public abstract class ReportViewerParameters
    {
        public string ReportServerUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReportPath { get; set; }
        public Guid? ControlId { get; set; }
        public bool IsAzureSsrs { get; set; }
        public bool IsGeradorReportViewerExecution { get; set; }
        public string EventsHandlerType { get; set; }
        public ControlSettings ControlSettings { get; internal set; }
    }
}
