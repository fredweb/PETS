using Microsoft.Reporting.WebForms;

namespace ReportViewBase.WebForms.Interfases
{
    public interface IReportViewerEventsHandler
    {
        void OnSubreportProcessing ( ReportViewer reportViewer, SubreportProcessingEventArgs e );
        void OnDrillthrough ( ReportViewer reportViewer, DrillthroughEventArgs e );
    }
}
