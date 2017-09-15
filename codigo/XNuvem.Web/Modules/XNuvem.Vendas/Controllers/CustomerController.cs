/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data;
using XNuvem.Vendas.Services;
using XNuvem.Vendas.DomainModels;
using System.Threading.Tasks;
using XNuvem.Security;

namespace XNuvem.Vendas.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly IDbConnection _connection;
        private readonly IDirectDataTable _dataTable;
        private readonly IUserService _userService;
        public CustomerController(
            IDbConnection connection, 
            IDirectDataTable dataTable,
            IUserService userService) {
            _dataTable = dataTable;
            _connection = connection;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult List() {
            return View();
        }

        [HttpPost]
        public ActionResult List(FormCollection data) {
            var user = _userService.GetCurrentUser();
            if (user == null) throw new XNuvemCoreException("Usuário não ativo.");
            _dataTable.SetParameters(data);
            var result = _dataTable.Execute<BusinessPartnerResume>(
                BusinessPartnerResume.SqlSelectDataTablesBySlpCode(user.SlpCode),
                new string[] { "CardCode", "CardName", "City", "State" });
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> Select2(string q, int start) {
            var user = _userService.GetCurrentUser();
            if (user == null) throw new XNuvemCoreException("Usuário não ativo.");
            var data = new Select2Result<BusinessPartnerResume>();
            data.q = q;
            data.total = await _connection.ExecuteScalarAsync<int>(BusinessPartnerResume.SqlCountCompleteBySlpCode(user.SlpCode), new { @q = string.Concat(q, "%") });
            data.start = start < 0 ? 0 : start;
            data.length = 30;
            data.results = await _connection.QueryAsync<BusinessPartnerResume>(BusinessPartnerResume.SqlSelectCompleteBySlpCode(user.SlpCode), new { @q = string.Concat(q, "%"), @start = data.start, @length = data.length });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string id) {
            var model = (await _connection.QueryAsync<BusinessPartner>(BusinessPartner.SqlSelectByCardCode, new { CardCode = id })).SingleOrDefault();
            if (model == null) {
                throw new XNuvemCoreException(string.Format("Valor fornecido no id {0} não é válido.", id));
            }
            model.Address = await _connection.QueryAsync<BusinessPartnerAddress>(BusinessPartnerAddress.SqlSelectByCardCode, new { CardCode = id });
            model.PayMethods = await _connection.QueryAsync<BusinessPartnerPayMethod>(BusinessPartnerPayMethod.SqlSelectByCardCode, new { CardCode = id });
            return View(model);
        }

        public async Task<ActionResult> Receivables(string id) {
            var model = await _connection.QueryAsync<BPReceivables>(BPReceivables.SqlSelectByCardCode, new { CardCode = id });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ChequesDevolvidos(string id) {
            var user = _userService.GetCurrentUser();
            if (user == null) throw new XNuvemCoreException("Usuário não ativo.");
            var model = await _connection.QueryAsync<BPReceivables>(BPReceivables.SqlChequesDevolvidos, new { SlpCode = user.SlpCode, CardCode = id });
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChequesDevolvidosTable(FormCollection data) {
            var user = _userService.GetCurrentUser();
            if (user == null) throw new XNuvemCoreException("Usuário não ativo.");
            _dataTable.SetParameters(data);
            var result = _dataTable.Execute<BPReceivables>(
                BPReceivables.GetSqlTodosChequesDevolvidos(user.SlpCode),
                new string[] { "CardCode", "CardName" });
            return Json(result);
        }

        [HttpGet]
        public ActionResult Cheques() {
            return View();
        }

        public async Task<ActionResult> CheckCredit(string id) {
            var model = (await _connection.QueryAsync<CreditBalance>(CreditBalance.SqlSelectByMaster, new { @CardCode = id })).SingleOrDefault();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}