/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using XNuvem.Vendas.DomainModels;
using XNuvem.Vendas.Models;
using XNuvem.Logging;
using XNuvem.Data;
using XNuvem.UI.DataTable;
using XNuvem.UI;
using System.Threading.Tasks;
using XNuvem.UI.Model;
using XNuvem.Vendas.Records;
using XNuvem.Vendas.Services;
using XNuvem.Vendas.Services.Models;
using XNuvem.B1;
using SAPbobsCOM;
using XNuvem.Security;

namespace XNuvem.Vendas.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IDictionary<string, string> _statusFilters = new Dictionary<string, string> {
            { "-1", "" },
            { "N", "AND (Approved = 0)" },
            { "NW", "AND (DocStatus = 'W')" },
            { "NO", "AND (Canceled = 'N' AND DocStatus = 'O')" },
            { "NC", "AND (Canceled = 'N' AND DocStatus = 'C')" },
            { "Y", "AND (Canceled = 'Y')" }
        };

        private readonly IUserService _userService;
        private readonly IDbConnection _connection;
        private readonly IRepository<OrderRecord> _orders;
        private readonly IRepository<OrderLineRecord> _orderLines;
        private readonly IB1ConfigurationManager _configurationManager;
        private readonly CompanySettings _companySettings;
        private readonly IDirectDataTable _dataTable;
        private readonly IRepository<UsageRecord> _usages;
        private readonly IB1Connection _sboConnection;

        public OrderController(
            IUserService userService,
            IDbConnection dbConnection, 
            IRepository<OrderRecord> orders,
            IRepository<OrderLineRecord> orderLines,
            IB1ConfigurationManager configurationManager,
            IDirectDataTable directDataTable,
            IRepository<UsageRecord> usages,
            IB1Connection sboConnection) {
                _userService = userService;
                _connection = dbConnection;
                _orders = orders;
                _orderLines = orderLines;
                _configurationManager = configurationManager;
                _companySettings = configurationManager.GetSettings();
                _dataTable = directDataTable;
                _usages = usages;
                _sboConnection = sboConnection;
        }

        [HttpGet]
        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        public ActionResult Create(OrderViewModel model) {
            if (!ModelState.IsValid) {
                return MessageAjax(true, "Verifique se as informações foram inseridas corretamente.");
            }

            // Custom validations
            if (model.BusinessPartner == null) {
                return MessageAjax(true, "Ocorreu um erro ao verificar o parceiro de negócio.");
            }
            if (model.FinancialDetails == null) {
                return MessageAjax(true, "Ocorreu um erro ao verificar os dados financeiros do parceiro de negócio.");
            }
            if (model.Approved && !model.FinancialDetails.Approved && string.IsNullOrEmpty(model.Comments)) {
                return MessageAjax(true, "Não foi inserido os comentários da aprovação deste parceiro de negócio.");
            }
            if (model.Lines == null) {
                return MessageAjax(true, "Não foi inserido um item para este pedido.");
            }
            if (model.Lines.Sum(m => m.Quantity) == 0D) {
                return MessageAjax(true, "A soma de quantidade do pedido não pode ser zero.");
            }
            if (model.Lines.Where(m => (m.Price * m.Quantity) <= 0D).Count() > 0) {
                return MessageAjax(true, "O pedido não pode haver linhas com valor menor ou igual a zero.");
            }

            var currentUser = _userService.GetCurrentUser();

            var order = new OrderRecord {
                SlpCode = currentUser.SlpCode,
                DocDate = DateTime.Now,
                CardCode = model.CardCode,
                CardName = model.BusinessPartner.CardName,
                RotaCode = model.RotaCode,
                RotaName = model.RotaName,
                GroupNum = model.GroupNum,
                PymntGroup = model.PymntGroup,
                PeyMethod = model.PeyMethod,
                ListNum = model.BusinessPartner.ListNum,
                ListName = model.ListName,
                Comments = model.Comments,
                InSbo = false,
                Approved = model.Approved,
                Usage = model.Usage,
                Carga = model.Carga,

                //TODO: Mover dados de usuário para um serviço
                CreatedBy = User.Identity.Name,
                CreatedAt = DateTime.Now,
                UpdatedBy = User.Identity.Name,
                UpdatedAt = DateTime.Now
            };
            order.Lines = model.Lines.Select(x => new OrderLineRecord {
                ItemCode = x.ItemCode,
                ItemName = x.ItemName,
                Order = order,
                Price = x.Price,
                Quantity = x.Quantity,
                LineTotal = (x.Price * x.Quantity)
            }).ToList();

            order.DocTotal = order.Lines.Sum(m => m.LineTotal);

            _orders.Create(order);

            return Json(new MessageResult {
                IsError = false,
                Messages = new List<string> { "Operação concluída com êxito." }
            });
        }

        public ActionResult Items(int listNum, int iDocEntry = 0) {
            var user = _userService.GetCurrentUser();

            IEnumerable<ItemLine> lines = null;

            if (iDocEntry == 0) {
                lines = _connection.Query<ItemLine>(ItemLine.SelectByPriceList, new { PriceList = listNum, Qry60 = user.QryGroup60, Qry61 = user.QryGroup61, Qry62 = user.QryGroup62 });
            }
            else {
                lines = _connection.Query<ItemLine>(string.Format(
                    ItemLine.SelectByPriceListOrder,
                    _companySettings.ApplicationDB), 
                    new { 
                        PriceList = listNum, 
                        @iDocEntry = iDocEntry, 
                        Qry60 = user.QryGroup60, 
                        Qry61 = user.QryGroup61, 
                        Qry62 = user.QryGroup62 });
            }

            Dictionary<string, ItemGroup> groupKey = new Dictionary<string, ItemGroup>();
            var emptyGroup = new ItemGroup { Name = "Sem grupo" };
            foreach (var item in lines) {
                ItemGroup currentGroup = null;
                if (string.IsNullOrEmpty(item.GroupName)) {
                    emptyGroup.Items.Add(item);
                }
                else if (groupKey.TryGetValue(item.GroupName, out currentGroup)) {
                    currentGroup.Items.Add(item);
                }
                else {
                    currentGroup = new ItemGroup { Name = item.GroupName };
                    currentGroup.Items.Add(item);
                    groupKey.Add(currentGroup.Name, currentGroup);
                }
            }
            if (emptyGroup.Items.Count > 0)
            {
                groupKey.Add("", emptyGroup);
            }
            var model = new ItemGroupViewModel { Groups = groupKey.Values.ToList() };
            return View(model);
        } //ActionResult Items(int listNum)

        [HttpGet]
        public ActionResult List() {
            return View();
        }

        [HttpPost]
        public ActionResult List(FormCollection data) {
            var user = _userService.GetCurrentUser();
            if (user == null) throw new XNuvemCoreException("Usuário não encontrado");
            var currentStatusFilter = data["StatusFilter"];
            var whereExtender = string.Format("({0} = -1 OR SlpCode = {0}) ", user.SlpCode) + _statusFilters[currentStatusFilter];

            _dataTable.SetParameters(data);
            var result = _dataTable.Execute<OrderResume>(
                OrderResume.GetSqlDirectTable(_companySettings.ApplicationDB),
                new string[] { "CardCode", "CardName", "RotaCode" }, whereExtender);
            return Json(result);
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            var order = _orders.Table.Where(o => o.iDocEntry == id).SingleOrDefault();

            var user = _userService.GetCurrentUser();
            if (user.SlpCode != -1 && user.SlpCode != order.SlpCode) {
                throw new XNuvemCoreException("Usuário sem permissão para alterar este pedido.");
            }

            var model = new OrderViewModel {
                iDocEntry = order.iDocEntry,
                DocEntry = order.DocEntry,
                DocDate = order.DocDate,
                CardCode = order.CardCode,
                Approved = order.Approved,
                GroupNum = order.GroupNum,
                PymntGroup = order.PymntGroup,
                PeyMethod = order.PeyMethod,
                RotaCode = order.RotaCode,
                RotaName = order.RotaName,
                ListName = order.ListName,
                Comments = order.Comments,
                BusinessPartner = _connection.Query<BusinessPartnerResume>(BusinessPartnerResume.SqlSelectByCardCode, new { CardCode = order.CardCode }).SingleOrDefault(),
                FinancialDetails = _connection.Query<CreditBalance>(CreditBalance.SqlSelectByMaster, new { CardCode = order.CardCode }).SingleOrDefault(),
                Usage = order.Usage,
                UsageText = GetUsageText(order.Usage),
                Carga = order.Carga
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(OrderViewModel model) {
            if (!ModelState.IsValid) {
                return MessageAjax(true, "Verifique se as informações foram inseridas corretamente.");
            }

            // Custom validations
            if (model.BusinessPartner == null) {
                return MessageAjax(true, "Ocorreu um erro ao verificar o parceiro de negócio.");
            }
            if (model.FinancialDetails == null) {
                return MessageAjax(true, "Ocorreu um erro ao verificar os dados financeiros do parceiro de negócio.");
            }
            if (model.Approved && !model.FinancialDetails.Approved && string.IsNullOrEmpty(model.Comments)) {
                return MessageAjax(true, "Não foi inserido os comentários da aprovação deste parceiro de negócio.");
            }
            if (model.Lines == null) {
                return MessageAjax(true, "Não foi inserido um item para este pedido.");
            }
            if (model.Lines.Sum(m => m.Quantity) == 0D) {
                return MessageAjax(true, "A soma de quantidade do pedido não pode ser zero.");
            }
            if (model.Lines.Where(m => (m.Price * m.Quantity) <= 0D).Count() > 0) {
                return MessageAjax(true, "O pedido não pode haver linhas com valor menor ou igual a zero.");
            }

            var order = _orders.Table.Where(o => o.iDocEntry == model.iDocEntry).SingleOrDefault();

            if (order == null) {
                return MessageAjax(true, "O pedido não foi encontrado.");
            }

            if (order.InSbo || order.DocEntry != 0) {
                return MessageAjax(true, "Este pedido já foi confirmado e não pode ser alterado.");
            }

            var user = _userService.GetCurrentUser();
            if (user.SlpCode != -1 && user.SlpCode != order.SlpCode) {
                return MessageAjax(true, "Este pedido não pode ser alterado por este usuário");
            }

            // Limpa as linhas para acomodar a nova entrada
            foreach (var line in order.Lines) {
                _orderLines.Delete(line);
            }
            order.Lines.Clear();

            order.SlpCode = model.BusinessPartner.SlpCode;
            order.CardCode = model.CardCode;
            order.CardName = model.BusinessPartner.CardName;
            order.RotaCode = model.RotaCode;
            order.RotaName = model.RotaName;
            order.GroupNum = model.GroupNum;
            order.PymntGroup = model.PymntGroup;
            order.PeyMethod = model.PeyMethod;
            order.ListNum = model.BusinessPartner.ListNum;
            order.ListName = model.ListName;
            order.Comments = model.Comments;
            order.Approved = model.Approved;
            order.Usage = model.Usage;
            order.Carga = model.Carga;

            order.UpdatedBy = User.Identity.Name;
            order.UpdatedAt = DateTime.Now;

            
            order.Lines = model.Lines.Select(x => new OrderLineRecord {
                ItemCode = x.ItemCode,
                ItemName = x.ItemName,
                Order = order,
                Price = x.Price,
                Quantity = x.Quantity,
                LineTotal = (x.Price * x.Quantity)
            }).ToList();

            order.DocTotal = order.Lines.Sum(m => m.LineTotal);

            _orders.Update(order);

            return MessageAjax(false, "Operação concluída com êxito.");
        }

        public ActionResult LastOrder(string cardCode) {
            var user = _userService.GetCurrentUser();
            if (user == null || user.SlpCode == 0) {
                throw new XNuvemCoreException("Não foi possível encontrar o usuário.");
            }
            var order = _orders.Table.Where(o => o.CardCode == cardCode && o.SlpCode == user.SlpCode).OrderByDescending(o => o.CreatedAt).Take(1).SingleOrDefault();
            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> PayMethods(string cardCode) {
            if (string.IsNullOrEmpty(cardCode)) return Json(new { }, JsonRequestBehavior.AllowGet);
            var data = new AjaxDropDownResult();
            var res = await _connection.QueryAsync<AjaxDropDownResultItem>("SELECT [PymCode] AS [id], [PymCode] AS [text] FROM CRD2 WHERE CardCode = @CardCode", new { @CardCode = cardCode });
            data.results = res.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> PayGroups(string cardCode, string q) {
            var sqlText = @"
SELECT 
	GroupNum [id],
	PymntGroup [text]
FROM (SELECT
	T1.GroupNum, 
	T1.PymntGroup, 
	ISNULL(T2.InstMonth, 0)
	InstMonth, 
	ISNULL(T2.InstDays, 0) InstDays,
	((ISNULL(T2.InstMonth, 0) * 30) + ISNULL(T2.InstDays, 0)) TotalDays
FROM 
	OCTG T1
	LEFT JOIN CTG1 T2 ON T2.CTGCode = T1.GroupNum AND T2.IntsNo = T1.InstNum
) _Table
WHERE 
	TotalDays <= ISNULL((SELECT U_BS_LimitePrazo FROM OCRD WHERE CardCode = @CardCode), 0)
	AND PymntGroup LIKE CONCAT(@q1, '%')
";
            var data = new AjaxDropDownResult();
            var res = await _connection.QueryAsync<AjaxDropDownResultItem>(sqlText, new { q1 = q, @CardCode = cardCode });
            data.results = res.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Rotas(string q) {
            var user = _userService.GetCurrentUser();
            var data = new AjaxDropDownResult();
            var res = await _connection.QueryAsync<AjaxDropDownResultItem>("SELECT Code AS [id], CONCAT(Code, '-', U_RotaName) AS [text] FROM  [@PRV_RM_OROTA] WHERE (@SlpCode = -1 OR U_SlpCode = @SlpCode) AND (Code LIKE @q1 OR U_RotaName LIKE @q1) ORDER BY 2 ASC", new { q1 = string.Concat(q, "%"), SlpCode = user.SlpCode });
            data.results = res.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Usages(string q) {
            var usages = _usages.Table.OrderBy(m => m.Name).ToList();
            usages.Insert(0, new UsageRecord { AbsEntry = -1, Name = "Padrão", Usage = -1, UsageText = "Padrão" });
            var data = new AjaxDropDownResult();
            data.results = usages.Where(u => u.Name.StartsWith(q ?? "")).Select(m => new AjaxDropDownResultItem {
                id = m.Usage.ToString(),
                text = m.Name
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ProcessOrder(OrderResume model) {
            var orderWeb = _orders.Table.Where(m => m.iDocEntry == model.iDocEntry).SingleOrDefault();
            // Validate order status
            if (orderWeb == null) {
                return MessageAjax(true, "Não foi possível encontrar o pedido.");
            }
            if (!orderWeb.Approved) {
                return MessageAjax(true, "Pedido não aprovado.");
            }
            if (orderWeb.InSbo || orderWeb.DocEntry != 0) {
                return MessageAjax(true, "Pedido já se encontra no SAP.");
            }
            if (orderWeb.Canceled != "N") {
                return MessageAjax(true, "Pedido se encontra cancelado.");
            }
            //var currentUser = _userService.GetCurrentUser();
            return await Task.Run<ActionResult>(() => {
                _sboConnection.Company.StartTransaction();
                try {
                    var orderSbo = _sboConnection.Company.GetBusinessObject(BoObjectTypes.oOrders) as Documents;
                    orderSbo.CardCode = orderWeb.CardCode;
                    orderSbo.DocDueDate = DateTime.Now;
                    orderSbo.Comments = orderWeb.Comments ?? "";
                    orderSbo.UserFields.Fields.Item("U_WB_RouteNumber").Value = orderWeb.RotaCode + (!string.IsNullOrWhiteSpace(orderWeb.Carga) ? string.Concat(".", orderWeb.Carga) : "");
                    orderSbo.TaxExtension.Incoterms = "0";
                    foreach (var line in orderWeb.Lines) {
                        orderSbo.Lines.ItemCode = line.ItemCode;
                        orderSbo.Lines.Quantity = line.Quantity;
                        //TODO: Corrigir a operação de Usage pois encontra-se incorreta, mudar para o uso do texto
                        if (orderWeb.Usage == -1) {
                            orderSbo.Lines.Usage = _connection.QuerySingle<string>("SELECT U_UsageV FROM OITM WHERE ItemCode = @ItemCode", new { ItemCode = line.ItemCode });
                        }
                        else {
                            orderSbo.Lines.Usage = orderWeb.Usage.ToString();
                        }
                        orderSbo.Lines.Add();
                    }

                    orderSbo.SalesPersonCode = orderWeb.SlpCode;
                    orderSbo.PaymentGroupCode = orderWeb.GroupNum;
                    orderSbo.PaymentMethod = orderWeb.PeyMethod;

                    var retCode = orderSbo.Add();
                    if (retCode != 0) {
                        if (_sboConnection.Company.InTransaction) {
                            _sboConnection.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }
                        var error = _sboConnection.Company.GetLastErrorDescription();
                        return MessageAjax(true, error);
                    }
                    var lastKey = _sboConnection.Company.GetNewObjectKey();
                    orderWeb.InSbo = true;
                    orderWeb.DocStatus = "O";
                    orderWeb.DocEntry = int.Parse(lastKey);
                    _orders.Update(orderWeb);
                    if (_sboConnection.Company.InTransaction) {
                        _sboConnection.Company.EndTransaction(BoWfTransOpt.wf_Commit);
                    }
                }
                catch (Exception) {
                    if (_sboConnection.Company.InTransaction) {
                        _sboConnection.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                        throw;
                    }
                }
                return MessageAjax(false, "Operação concluída com êxito.");
            });
        }

        public ActionResult Delete(string keys) {
            var keyArray = keys.Split(';').Select(s => int.Parse(s));
            var orders = _orders.Table.Where(m => keyArray.Contains(m.iDocEntry)).ToList();
            foreach (var order in orders) {
                if (order.InSbo || order.DocEntry != 0) {
                    return MessageAjax(true, string.Format("O pedido {0} não pode ser excluído pois ele se encontra no SAP.", order.iDocEntry));
                }
            }
            foreach (var order in orders) {
                _orders.Delete(order);
            }
            return MessageAjax(false, "Operação concluída com êxito.");
        }

        private ActionResult MessageAjax(bool isError, string message) {
            return Json(new MessageResult {
                IsError = isError,
                Messages = new List<string> { message }
            }); 
        }

        private string GetUsageText(int usage) {
            if(usage == -1 || usage == 0) return "Padrão";
            var firstUsage = _usages.Table.Where(m => m.Usage == usage).Take(1).SingleOrDefault();
            return firstUsage.Name;
        }

        public ActionResult StatusDetail(int docEntry, int iDocEntry) {
            var order = _connection.Query<OrderDetail>(
                string.Format(OrderDetail.SqlSelectByDocEntry, _companySettings.ApplicationDB, _companySettings.CompanyDB),
                new { iDocEntry = @iDocEntry, DocEntry = @docEntry }
                ).SingleOrDefault();

            return Json(order);
        }
    }
}