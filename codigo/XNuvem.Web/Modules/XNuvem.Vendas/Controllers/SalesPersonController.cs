using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XNuvem.UI;
using Dapper;

namespace XNuvem.Vendas.Controllers
{
    public class SalesPersonController : Controller
    {
        private readonly IDbConnection _connection;
        public SalesPersonController(IDbConnection connection) {
            _connection = connection;
        }

        public async Task<ActionResult> List(string q) {
            var data = new AjaxDropDownResult();
            var res = await _connection.QueryAsync<AjaxDropDownResultItem>("SELECT SlpCode [id], SlpName [text] FROM OSLP WHERE SlpName LIKE @q1 ORDER BY 1 ASC", new { q1 = string.Concat(q, "%") });
            data.results = res.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}