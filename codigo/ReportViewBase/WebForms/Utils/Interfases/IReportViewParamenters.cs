using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace ReportViewBase.WebForms.Utils.Interfases
{
    public interface IReportViewParamenters
    {
        DataSourceCredentials [ ] DataSourceCredentials { get; set; }
        ProcessingMode ProcessingMode { get; set; }
        IDictionary<string, ReportParameter> ReportParameters { get; set; }
        IDictionary<string, DataTable> LocalReportDataSources { get; set; }
        IDictionary<string, IEnumerable> LocalReportDatasourceEnumerable { get; set; }
        IDictionary<string, IDataSource> LocalReportIDataSource { get; set; }
        Stream EmbeddedResourceStream { get; set; }
        string EmbeddedResourcePath { get; set; }
        string LocalPath { get; set; }
    }
}
