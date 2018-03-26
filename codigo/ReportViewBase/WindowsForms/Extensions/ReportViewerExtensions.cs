using System;
using System.Linq;
using Microsoft.Reporting.WinForms;
using ReportViewBase.Base.Exception;
using ReportViewBase.Base.Utils;
using ReportViewBase.WindowsForms.Interfases;
using ReportViewBase.WindowsForms.Util;

namespace ReportViewBase.WindowsForms.Extensions
{
    public static class ReportViewerExtensions
    {
        public static void Initialize(this ReportViewer reportViewer, ReportViewParamenters parameters)
        {
            var handlers = CreateEventHandlers(parameters);
            SetupLocalProcessing(reportViewer, parameters, handlers);
            if (handlers != null) reportViewer.Drillthrough += (sender, e) => handlers.OnDrillthrough(reportViewer, e);
        }

        public static void SetupDrillthrough(this ReportViewer reportViewer, string eventsHandlerType)
        {
            var handlers = CreateEventHandlers(eventsHandlerType);
            if (handlers != null) reportViewer.Drillthrough += (sender, e) => handlers.OnDrillthrough(reportViewer, e);
        }

        private static void SetupLocalProcessing(ReportViewer reportViewer, ReportViewParamenters parameters,
            IReportViewerEventsHandler handlers)
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var localReport = reportViewer.LocalReport;

            if (!string.IsNullOrWhiteSpace(parameters.EmbeddedResourcePath))
                localReport.ReportEmbeddedResource = parameters.EmbeddedResourcePath;
            else if (!string.IsNullOrWhiteSpace(parameters.LocalPath))
                localReport.ReportPath = parameters.LocalPath;
            else
                localReport.LoadReportDefinition(parameters.EmbeddedResourceStream);
            if (parameters.ControlSettings != null &&
                parameters.ControlSettings.UseCurrentAppDomainPermissionSet != null &&
                parameters.ControlSettings.UseCurrentAppDomainPermissionSet.Value)
                localReport.SetBasePermissionsForSandboxAppDomain(AppDomain.CurrentDomain.PermissionSet.Copy());

            if (parameters.ControlSettings != null && parameters.ControlSettings.EnableExternalImages != null &&
                parameters.ControlSettings.EnableExternalImages.Value)
                localReport.EnableExternalImages = true;

            if (parameters.ControlSettings != null && parameters.ControlSettings.EnableHyperlinks != null &&
                parameters.ControlSettings.EnableHyperlinks.Value)
                localReport.EnableHyperlinks = true;

            if (parameters.ReportParameters.Count > 0)
                localReport.SetParameters(parameters.ReportParameters.Values.ToList());

            if (handlers != null)
                localReport.SubreportProcessing += (sender, e) => handlers.OnSubreportProcessing(reportViewer, e);

            // If parameters.LocalReportDataSources then we should get report data source
            // from local data source provider (ignore it for Report Runner)
            if (parameters.LocalReportDataSources == null && !parameters.IsGeradorReportViewerExecution)
            {
                if (parameters.ControlId == null) throw new ReportException("ReportViewer control ID is missing");
                localReport.DataSources.Clear();
                foreach (var dataSource in parameters.LocalReportDataSources)
                {
                    var reportDatasource = new ReportDataSource(dataSource.Key) {Value = dataSource.Value};
                    localReport.DataSources.Add(reportDatasource);
                }
            }
        }

        private static IReportViewerEventsHandler CreateEventHandlers(ReportViewerParameters parameters)
        {
            return CreateEventHandlers(parameters.EventsHandlerType);
        }

        private static IReportViewerEventsHandler CreateEventHandlers(string eventsHandlerType)
        {
            if (string.IsNullOrEmpty(eventsHandlerType)) return null;

            var handlersType = Type.GetType(eventsHandlerType);
            if (handlersType == null)
                throw new ReportException(string.Format("Type {0} is not found", eventsHandlerType));

            var handlers = Activator.CreateInstance(handlersType) as IReportViewerEventsHandler;
            if (handlers == null)
                throw new ReportException(
                    string.Format(
                        "Type {0} has not implemented IReportViewerEventsHandler interface or cannot be instantiated.",
                        eventsHandlerType));

            return handlers;
        }
    }
}