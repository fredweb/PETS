using Microsoft.Reporting.WinForms;

namespace ReportViewBase.WindowsForms.Interfases
{
    public interface IReportViewerEventsHandler
    {
        void OnSubreportProcessing(ReportViewer reportViewer, SubreportProcessingEventArgs e);
        void OnDrillthrough(ReportViewer reportViewer, DrillthroughEventArgs e);
    }
}