/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2017
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
using XNuvem.Data;
using XNuvem.Vendas.Records;
using XNuvem.Vendas.Models;
using XNuvem.UI.Model;

namespace XNuvem.Vendas.Controllers
{
    [Authorize]
    public class CobrancaController : Controller
    {
        private readonly IDbConnection _connection;
        private readonly IDirectDataTable _dataTable;
        private readonly IUserService _userService;
        private readonly IRepository<AtividadeRecord> _atividades;

        public CobrancaController(
            IDbConnection connection, 
            IDirectDataTable dataTable,
            IUserService userService,
            IRepository<AtividadeRecord> atividades) {
            _dataTable = dataTable;
            _connection = connection;
            _userService = userService;
            _atividades = atividades;
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
            var result = _dataTable.Execute<Cobranca>(
                Cobranca.SelectBySlpCode(user.SlpCode),
                new string[] { "CardCode", "CardName", "City", "State", "SlpName" });
            return Json(result);
        }

        [HttpGet]
        public ActionResult ListVencidos() {
            return View();
        }

        [HttpPost]
        public ActionResult ListVencidos(FormCollection data) {
            var user = _userService.GetCurrentUser();
            if (user == null) throw new XNuvemCoreException("Usuário não ativo.");
            _dataTable.SetParameters(data);
            var result = _dataTable.Execute<Cobranca>(
                Cobranca.SelectVencidosBySlpCode(user.SlpCode),
                new string[] { "CardCode", "CardName", "City", "State", "SlpName" });
            return Json(result);
        }

        [HttpGet]
        public ActionResult List60() {
            return View();
        }

        [HttpPost]
        public ActionResult List60(FormCollection data) {
            var user = _userService.GetCurrentUser();
            if (user == null) throw new XNuvemCoreException("Usuário não ativo.");
            _dataTable.SetParameters(data);
            var result = _dataTable.Execute<Cobranca>(
                Cobranca.Select60BySlpCode(user.SlpCode),
                new string[] { "CardCode", "CardName", "City", "State", "SlpName" });
            return Json(result);
        }

        [HttpPost]
        public ActionResult CreateAtividade(AtividadeViewModel model) {
            if (!ModelState.IsValid) {
                return MessageAjax(true, "Informações inconsistentes.");
            }

            var record = new AtividadeRecord {
                DocType = model.DocType,
                DocNum = model.DocNum,
                CardCode = model.CardCode,
                Comments = model.Comments,
                CreatedAt = DateTime.Now,
                CreatedBy = User.Identity.Name,
                UpdatedAt = DateTime.Now,
                UpdatedBy = User.Identity.Name
            };

            _atividades.Create(record);

            return MessageAjax(false, "Operação concluída com êxito.");
        }

        [HttpGet]
        public ActionResult AtividadeList(string doctype, int docnum, string cardcode) {
            var list = _atividades.Table.Where(x => x.DocType == doctype && x.DocNum == docnum && x.CardCode == cardcode).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private ActionResult MessageAjax(bool isError, string message) {
            return Json(new MessageResult {
                IsError = isError,
                Messages = new List<string> { message }
            });
        }
    }
}