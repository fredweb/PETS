using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XNuvem.UI;
using Dapper;
using XNuvem.Logistica.Models.SalesReportViewModels;
using XNuvem.Logistica.Services;

namespace XNuvem.Logistica.Controllers
{
    [Authorize]
    public class SalesReportController : Controller
    {
        private readonly IDbConnection _connection;
        private readonly IB1ConfigurationManager _configurationManager;

        public SalesReportController(IDbConnection connection,
            IB1ConfigurationManager configurationManager) {
            _connection = connection;
            _configurationManager = configurationManager;
        }

        // GET: SalesReport/Cities
        [HttpGet]
        public ActionResult Cities() {
            var model = new SalesReportCitiesModel();
            model.From = DateTime.Now.AddMonths(-1).Date;
            model.To = DateTime.Now.Date;
            model.SlpCode = -1;
            model.SlpName = "-Nenhum Vendedor-";
            return View(model);
        }

        [HttpPost]
        public ActionResult Cities(SalesReportCitiesModel model) {
            var sql = @"
SELECT
	IIF(Q1.IbgeCode IS NULL, 'N', 'Y') HasData,   
	Q1.IbgeCode PIbgeCode,
	Q1.State PState,
	Q1.City PCity,
	Q1.Populacao PPopulacao,
	Q1.Lat PLat,
	Q1.Lng PLng,
	Q1.DocTotal PDocTotal,	
	Q2.IbgeCode SIbgeCode,
	Q2.State SState,
	Q2.City SCity,
	Q2.Populacao SPopulacao,
	Q2.Lat SLat,
	Q2.Lng SLng,
	Q2.DocTotal SDocTotal
FROM (
SELECT 
	T4.IbgeCode, T4.State, T4.Name City, T5.Populacao, ISNULL(T5.Latitude, 0) Lat, ISNULL(T5.Longitude, 0) Lng, SUM(T2.DocTotal) DocTotal
FROM
	OINV T2
	LEFT JOIN CRD1 T3 ON T3.AdresType = 'S' AND T3.CardCode = T2.CardCode AND T3.Address = T2.ShipToCode
	LEFT JOIN OCNT T4 ON CAST(T4.AbsId AS NVARCHAR) = T3.County
	LEFT JOIN [{0}]..[Cidades] T5 ON T5.IbgeCode = T4.IbgeCode
	LEFT JOIN OSLP T6 ON T6.SlpCode = T2.SlpCode
WHERE 
	T2.CANCELED = 'N' AND (@SlpCode = -1 OR T2.SlpCode = @SlpCode)  AND T2.DocDate BETWEEN @FromDate AND @ToDate
GROUP BY T4.IbgeCode, T4.State, T4.Name, T5.Populacao, T5.Latitude, T5.Longitude
) Q1
FULL JOIN
(
SELECT 
	T4.IbgeCode, T4.State, T4.Name City, T5.Populacao, ISNULL(T5.Latitude, 0) Lat, ISNULL(T5.Longitude, 0) Lng, SUM(T2.DocTotal) DocTotal
FROM
	OINV T2
	LEFT JOIN CRD1 T3 ON T3.AdresType = 'S' AND T3.CardCode = T2.CardCode AND T3.Address = T2.ShipToCode
	LEFT JOIN OCNT T4 ON CAST(T4.AbsId AS NVARCHAR) = T3.County
	LEFT JOIN [{0}]..[Cidades] T5 ON T5.IbgeCode = T4.IbgeCode
	LEFT JOIN OSLP T6 ON T6.SlpCode = T2.SlpCode
WHERE 
	T2.CANCELED = 'N' AND (@SlpCode = -1 OR T2.SlpCode = @SlpCode) AND T2.DocDate BETWEEN DATEADD(MONTH, -3, GETDATE()) AND GETDATE()
GROUP BY T4.IbgeCode, T4.State, T4.Name, T5.Populacao, T5.Latitude, T5.Longitude
) Q2 ON Q2.IbgeCode = Q1.IbgeCode
";

            var settings = _configurationManager.GetSettings();
            var result = _connection.Query<CityReport>(String.Format(sql, settings.ApplicationDB), new { SlpCode = model.SlpCode, FromDate = model.From, ToDate = model.To });
            return Json(result, JsonRequestBehavior.AllowGet);

        } //ActionResult Cities(SalesReportCitiesModel model)

        public async Task<ActionResult> SlpList(string q) {
            var data = new AjaxDropDownResult();
            var res = await _connection.QueryAsync<AjaxDropDownResultItem>("SELECT SlpCode [id], SlpName [text] FROM OSLP WHERE SlpName LIKE @q1 ORDER BY 1 ASC", new { q1 = string.Concat(q, "%") });
            data.results = res.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}