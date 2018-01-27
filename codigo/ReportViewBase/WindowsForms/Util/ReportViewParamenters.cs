using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using ReportViewBase.Base.Utils;
using Microsoft.Reporting.WinForms;

namespace ReportViewBase.WindowsForms.Util
{
    public class ReportViewParamenters : ReportViewerParameters
    {
        public ReportViewParamenters ( )
        {
            ReportParameters = new Dictionary<string, ReportParameter> ( );
        }

        public DataSourceCredentials [ ] DataSourceCredentials { get; set; }
        public IDictionary<string, ReportParameter> ReportParameters { get; set; }
        public IDictionary<string, DataTable> LocalReportDataSources { get; set; }
        public IDictionary<string, IEnumerable> LocalReportDatasourceEnumerable { get; set; }
        public IDictionary<string, IDataSource> LocalReportIDataSource { get; set; }
        public ProcessingMode ProcessingMode { get; set; }
        public Stream EmbeddedResourceStream { get; set; }
        public string EmbeddedResourcePath { get; set; }
        public string LocalPath { get; set; }
    }
}
