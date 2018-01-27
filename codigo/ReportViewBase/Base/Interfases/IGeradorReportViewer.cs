using System.IO;

namespace ReportViewBase.Base.Interfases
{
    public interface IGeradorReportViewer
    {
        string Extension { get; }
        string Filename { get; }
        string MimeType { get; }
        byte [ ] GetByteReport ( );
        Stream GetStreamReport ( );
    }
}
