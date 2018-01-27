using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using ReportViewBase.Base.Enum;
using ReportViewBase.Base.Exception;
using ReportViewBase.Base.Interfases;
using ReportViewBase.Base.Utils;
using ReportViewBase.WebForms.Extensions;
using ReportViewBase.WebForms.Utils;

namespace ReportViewBase.WebForms
{
    public class GeradorReportViewer : IGeradorReportViewer
    {
        private readonly ReportViewParamenters _viewerParameters;
        public GeradorReportViewer (
            Formato reportFormat,
            string reportPath,
            ModoProcessamento mode = ModoProcessamento.Remote,
            IDictionary<string, DataTable> localReportDataSources = null,
            IDictionary<string, IEnumerable> localReportEnumerable = null,
            IDictionary<string, IDataSource> localReportUiDataSource = null,
            string filename = null,
            string eventsHandlerType = null,
            Stream embeddedResourceStream = null,
            string embeddedResourcePath = null
        )
            : this ( reportFormat,
                reportPath,
                null,
                null,
                null,
                null,
                mode,
                localReportDataSources,
                localReportEnumerable,
                localReportUiDataSource,
                filename,
                eventsHandlerType,
                embeddedResourceStream,
                embeddedResourcePath
            )
        {
        }

        public GeradorReportViewer (
            Formato reportFormat,
            string reportPath,
            ModoProcessamento mode = ModoProcessamento.Remote,
            IEnumerable<KeyValuePair<string, object>> reportParameters = null,
            IDictionary<string, DataTable> localReportDataSources = null,
            IDictionary<string, IEnumerable> localReportEnumerable = null,
            IDictionary<string, IDataSource> localReportUiDataSource = null,
            string filename = null,
            string eventsHandlerType = null,
            Stream embeddedResourceStream = null,
            string embeddedResourcePath = null
        )
            : this ( reportFormat,
                reportPath,
                null,
                null,
                null,
                reportParameters,
                mode,
                localReportDataSources,
                localReportEnumerable,
                localReportUiDataSource,
                filename,
                eventsHandlerType,
                embeddedResourceStream,
                embeddedResourcePath
            )
        {
        }

        public GeradorReportViewer (
            Formato reportFormat,
            string reportPath,
            string reportServerUrl,
            string username,
            string password,
            ModoProcessamento mode = ModoProcessamento.Remote,
            IEnumerable<KeyValuePair<string, object>> reportParameters = null,
            IDictionary<string, DataTable> localReportDataSources = null,
            IDictionary<string, IEnumerable> localReportEnumerable = null,
            IDictionary<string, IDataSource> localReportUiDataSource = null,
            string filename = null,
            string eventsHandlerType = null,
            Stream embeddedResourceStream = null,
            string embeddedResourcePath = null
        )
            : this (
                reportFormat,
                reportPath,
                reportServerUrl,
                username,
                password,
                reportParameters,
                mode,
                localReportDataSources,
                localReportEnumerable,
                localReportUiDataSource,
                filename,
                eventsHandlerType,
                embeddedResourceStream,
                embeddedResourcePath
            )
        {
        }

        public GeradorReportViewer (
            Formato reportFormat,
            string reportPath,
            string reportServerUrl,
            string username,
            string password,
            IEnumerable<KeyValuePair<string, object>> reportParameters = null,
            ModoProcessamento mode = ModoProcessamento.Remote,
            IDictionary<string, DataTable> localReportDataSources = null,
            IDictionary<string, IEnumerable> localReportEnumerable = null,
            IDictionary<string, IDataSource> localReportUiDataSource = null,
            string filename = null,
            string eventsHandlerType = null,
            Stream embeddedResourceStream = null,
            string embeddedResourcePath = null
        )
        {
            _viewerParameters = new ReportViewParamenters
            {
                IsGeradorReportViewerExecution = true
            };


            _reportFormat = reportFormat;
            Filename = filename;

            if ( mode == ModoProcessamento.Local && localReportDataSources != null )
                _viewerParameters.LocalReportDataSources = localReportDataSources;
            else if ( mode == ModoProcessamento.Local && localReportEnumerable != null )
                _viewerParameters.LocalReportDatasourceEnumerable = localReportEnumerable;
            else if ( mode == ModoProcessamento.Local && localReportUiDataSource != null )
                _viewerParameters.LocalReportIDataSource = localReportUiDataSource;
            if ( !string.IsNullOrEmpty ( eventsHandlerType ) )
                _viewerParameters.EventsHandlerType = eventsHandlerType;

            _viewerParameters.EmbeddedResourceStream = embeddedResourceStream;
            _viewerParameters.LocalPath = reportPath;
            _viewerParameters.EmbeddedResourcePath = embeddedResourcePath;

            ParseParameters ( reportParameters );
        }


        public ReportViewerParameters ViewerParameters
        {
            get
            {
                return _viewerParameters;
            }
        }

        private readonly Formato _reportFormat;
        public string Filename { get; private set; }
        public string MimeType { get; private set; }
        public string Extension { get; private set; }
        public byte [ ] GetByteReport ( )
        {
            byte[] retuns = null;
            var reportViewer = new ReportViewer ( );
            reportViewer.Initialize ( _viewerParameters );
            ValidarFormato ( reportViewer );
            string mimeType;
            Stream output;
            if ( _viewerParameters.ProcessingMode == ProcessingMode.Remote )
            {
                string extension;
                var format = ReportFormat2String ( _reportFormat );

                output = reportViewer.ServerReport.Render (
                    format,
                    "<DeviceInfo></DeviceInfo>",
                    null,
                    out mimeType,
                    out extension );
            }
            else
            {
                var localReport = reportViewer.LocalReport;
                if ( _viewerParameters.LocalReportDataSources != null )
                    foreach ( var dataSource in _viewerParameters.LocalReportDataSources )
                    {
                        var reportDataSource = new ReportDataSource ( dataSource.Key, dataSource.Value );
                        localReport.DataSources.Add ( reportDataSource );
                    }
                else if ( _viewerParameters.LocalReportDatasourceEnumerable != null )
                    foreach ( var dataSource in _viewerParameters.LocalReportDatasourceEnumerable )
                    {
                        var reportDataSource = new ReportDataSource ( dataSource.Key, dataSource.Value );
                        localReport.DataSources.Add ( reportDataSource );
                    }
                else if ( _viewerParameters.LocalReportIDataSource != null )
                    foreach ( var dataSource in _viewerParameters.LocalReportIDataSource )
                    {
                        var reportDataSource = new ReportDataSource ( dataSource.Key, dataSource.Value );
                        localReport.DataSources.Add ( reportDataSource );
                    }

                Warning[] warnings;
                string[] streamids;
                string encoding;
                string extension;

                var format = ReportFormat2String ( _reportFormat );

                var report = localReport.Render (
                    format,
                    null,
                    out mimeType,
                    out encoding,
                    out extension,
                    out streamids,
                    out warnings );

                MimeType = mimeType;
                Extension = extension;
                retuns = report;
            }

            return retuns;
        }
        public Stream GetStreamReport ( )
        {
            return new MemoryStream ( GetByteReport ( ) );
        }


        private void ParseParameters ( IEnumerable<KeyValuePair<string, object>> reportParameters )
        {
            if ( reportParameters == null ) return;

            foreach ( var reportParameter in reportParameters )
            {
                var parameterName = reportParameter.Key;
                var parameterValue = reportParameter.Value;
                var parameterList = parameterValue as IEnumerable;
                if ( parameterList != null && !( parameterValue is string ) )
                {

                    foreach ( var value in parameterList )
                        if ( _viewerParameters.ReportParameters.ContainsKey ( parameterName ) )
                        {
                            _viewerParameters.ReportParameters [ parameterName ].Values.Add ( value.ToString ( ) );
                        }
                        else
                        {
                            var parameter = new ReportParameter ( parameterName );
                            parameter.Values.Add ( value.ToString ( ) );
                            _viewerParameters.ReportParameters.Add ( parameterName, parameter );
                        }
                }
                else
                {

                    if ( _viewerParameters.ReportParameters.ContainsKey ( parameterName ) )
                    {
                        if ( reportParameter.Value != null )
                            _viewerParameters.ReportParameters [ parameterName ].Values.Add ( reportParameter.Value.ToString ( ) );
                    }
                    else
                    {
                        var parameter = new ReportParameter ( parameterName );
                        parameter.Values.Add ( reportParameter.Value.ToString ( ) );
                        _viewerParameters.ReportParameters.Add ( parameterName, parameter );
                    }
                }
            }
        }

        private string ReportFormat2String ( Formato format )
        {
            return format == Formato.Html ? "HTML4.0" : format.ToString ( );
        }

        /// <summary>
        ///     Verificar se o formato é suportado no metodo Render do ReportView
        /// </summary>
        /// <param name="reportViewer"></param>
        private void ValidarFormato ( ReportViewer reportViewer )
        {
            var format = ReportFormat2String ( _reportFormat );
            var formats = _viewerParameters.ProcessingMode == ProcessingMode.Remote
                ? reportViewer.ServerReport.ListRenderingExtensions ( )
                : reportViewer.LocalReport.ListRenderingExtensions ( );

            if ( formats.All ( f => string.Compare ( f.Name, format, StringComparison.InvariantCultureIgnoreCase ) != 0 ) )
                throw new ReportException ( string.Format ( "{0} is not supported", _reportFormat ) );
        }
    }
}
