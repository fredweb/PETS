using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using XNuvem.Logistica.Models;
using XNuvem.Logistica.Services;

namespace XNuvem.Logistica.Controllers
{
    [Authorize]
    public class MapsController : Controller
    {
        private readonly IDbConnection _connection;
        private readonly IB1ConfigurationManager _configurationManager;
        public MapsController(
            IDbConnection connection,
            IB1ConfigurationManager configurationManager) {
            _connection = connection;
            _configurationManager = configurationManager;
        }

        // GET: Maps
        public ActionResult Route() {
            return View();
        }

        public ActionResult CityList(int romaneioEntry = 0) {
            var sql = @"
SELECT 
	T1.DocEntry, T1.LineId, T4.Country, T4.State, T4.Name, 0.0 Lat, 0.0 Lng, 0.0 AS DocTotal
FROM
	[@PRV_RM_DROM1] T1
	LEFT JOIN ORDR T2 ON T2.DocEntry = T1.U_PedNum
	LEFT JOIN CRD1 T3 ON T3.AdresType = 'S' AND T3.CardCode = T2.CardCode AND T3.Address = T2.ShipToCode
	LEFT JOIN OCNT T4 ON T4.AbsId = T3.County
WHERE T1.DocEntry = @DocEntry
ORDER BY T1.DocEntry ASC, T1.LineId ASC

";
            var query = _connection.Query<RomaneioCity>(sql, new { DocEntry = romaneioEntry });
            List<RomaneioCity> result = new List<RomaneioCity>();
            foreach (var city in query) {
                if (!result.Where(x => x.Name == city.Name && x.State == city.State).Any()) {
                    result.Add(city);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CityWithValue(int romaneioEntry = 0) {
            var sql = @"

SELECT 
	T1.DocEntry, 0 AS LineId, T4.Country, T4.State, T4.Name, ISNULL(T5.Latitude, 0) Lat, ISNULL(T5.Longitude, 0) Lng, SUM(T2.DocTotal) DocTotal
FROM
	[@PRV_RM_DROM1] T1
	LEFT JOIN ORDR T2 ON T2.DocEntry = T1.U_PedNum
	LEFT JOIN CRD1 T3 ON T3.AdresType = 'S' AND T3.CardCode = T2.CardCode AND T3.Address = T2.ShipToCode
	LEFT JOIN OCNT T4 ON T4.AbsId = T3.County
	LEFT JOIN [{0}]..[Cidades] T5 ON T5.IbgeCode = T4.IbgeCode
WHERE T1.DocEntry = @DocEntry
GROUP BY T1.DocEntry, T4.Country, T4.State, T4.Name, T5.Latitude, T5.Longitude
";
            var settings = _configurationManager.GetSettings();
            var result = _connection.Query<RomaneioCity>(String.Format(sql, settings.ApplicationDB) , new { DocEntry = romaneioEntry });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cities() {
            return View();
        }
    }
}