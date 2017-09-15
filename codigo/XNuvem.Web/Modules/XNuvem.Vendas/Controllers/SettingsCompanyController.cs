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
using XNuvem.B1;
using XNuvem.Environment.Configuration;
using XNuvem.Mvc;
using XNuvem.UI.Model;
using XNuvem.Vendas.Models;
using XNuvem.Vendas.Services;
using XNuvem.Vendas.Services.Models;
using Dapper;
using XNuvem.Vendas.DomainModels;
using XNuvem.Vendas.Records;
using XNuvem.Data;
using XNuvem.UI.DataTable;

namespace XNuvem.Vendas.Controllers
{
    [Authorize]
    [Admin("XNuvem.Vendas", "company")]
    public class SettingsCompanyController : Controller
    {
        private readonly IB1ConfigurationManager _configurationManager;
        private readonly IDbConnection _connection;
        private readonly IRepository<UsageRecord> _usageRecords;
        private readonly IJDataTable<UsageRecord> _usageDataTable;

        public SettingsCompanyController(
            IB1ConfigurationManager configurationManager, 
            IDbConnection connection,
            IRepository<UsageRecord> usageRecords,
            IJDataTable<UsageRecord> usageDataTable) {
            _configurationManager = configurationManager;
            _connection = connection;
            _usageRecords = usageRecords;
            _usageDataTable = usageDataTable;

            _usageDataTable.SearchOn("Name");
            _usageDataTable.SearchOn("UsageText");
        }

        [HttpGet]
        public ActionResult List() {
            return View();
        }

        [HttpGet]
        public ActionResult Edit() {
            var settings = _configurationManager.GetSettings();
            var model = new CompanyViewModel {
                Name = settings.Name,
                ConnectionString = settings.ConnectionString,
                ServerName = settings.ServerName,
                CompanyDB = settings.CompanyDB,
                UserName = settings.UserName,
                Password = settings.Password,
                MinPoolSize = int.Parse(settings.MinPoolSize),
                MaxPoolSize = int.Parse(settings.MaxPoolSize),
                ApplicationDB = settings.ApplicationDB
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CompanyViewModel model) {
            if (!ModelState.IsValid) {
                var erros = ModelState.Values.SelectMany(m => m.Errors).ToList();
                var messages = new List<string>(new[] { "Verifique se as informações foram digitadas corretamente." });
                messages.AddRange(erros.Select(m => m.ErrorMessage).ToList());
                return Json(new MessageResult {
                    IsError = true,
                    Messages = messages
                });
            }

            var settings = new CompanySettings {
                Name = model.Name,
                ConnectionString = model.ConnectionString,
                ServerName = model.ServerName,
                CompanyDB = model.CompanyDB,
                MinPoolSize = model.MinPoolSize.ToString(),
                MaxPoolSize = model.MaxPoolSize.ToString(),
                UserName = model.UserName,
                Password = model.Password,
                ApplicationDB = model.ApplicationDB
            };

            _configurationManager.StoreSettings(settings);

            return Json(new MessageResult {
                IsError = false,
                Messages = new[] { "Operação concluída com êxito." }
            });
        }

        [HttpPost]
        public ActionResult Test(CompanySettings model) {
            if (!ModelState.IsValid) {
                var erros = ModelState.Values.SelectMany(m => m.Errors).ToList();
                var messages = new List<string>(new[] { "Verifique se as informações foram digitadas corretamente." });
                messages.AddRange(erros.Select(m => m.ErrorMessage).ToList());
                return Json(new MessageResult {
                    IsError = true,
                    Messages = messages
                });
            }

            var parameters = new B1ConnectionParams{
                Server = model.ServerName,
                CompanyDB = model.CompanyDB,
                UserName = model.UserName,
                Password = model.Password,
                MinPoolSize = 1, 
                MaxPoolSize = 1
            };

            try {
                using (var connection = new B1Connection(parameters)) {
                    // connect on contructor
                }
            }
            catch (B1Exception ex) {
                return Json(new MessageResult {
                    IsError = true,
                    Messages = new[] { ex.Message }
                });
            }
            
            return Json(new MessageResult {
                IsError = false,
                Messages = new[] { "Conexão efetuada com êxito" }
            });
        } //  ActionResult Test(CompanySettings model)

        [HttpGet]
        public ActionResult Usage(int? id) {
            if (id.HasValue) {
                var model = _usageRecords.Table.Where(m => m.AbsEntry == id.Value).SingleOrDefault();
                if (model == null) {
                    throw new ArgumentException("id");
                }
                return View(new UsageViewModel {
                    Id = model.AbsEntry,
                    Name = model.Name,
                    Usage = model.Usage,
                    UsageText = model.UsageText
                });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Usage(UsageViewModel model) {
            if (!ModelState.IsValid) {
                return Json(new MessageResult {
                    IsError = true,
                    Messages = new[] { "Verifique se as informações estão corretas." }
                });
            }

            // Update
            if (model.Id.HasValue) { 
                var usage = _usageRecords.Table.Where(m => m.AbsEntry == model.Id.Value).SingleOrDefault();
                usage.Name = model.Name;
                usage.Usage = model.Usage;
                usage.UsageText = model.UsageText;
                _usageRecords.Update(usage);
            }
            // Create
            else { 
                var usage = new UsageRecord {
                    Name = model.Name,
                    Usage = model.Usage,
                    UsageText = model.UsageText
                };

                _usageRecords.Create(usage);
            }            

            return Json(new MessageResult {
                IsError = false,
                Messages = new[] { "Operação concluída com êxito." }
            });
        }
        

        public ActionResult Select2Usage(string q) {
            var usages = _connection.Query<Select2RecordBase>("SELECT ID AS [id], Usage AS [text] FROM OUSG WHERE Usage LIKE @q ORDER BY Usage ASC", new { @q = q + "%" });
            var result = new Select2Result<Select2RecordBase> {
                start = 0,
                length = usages.Count(),
                results = usages,
                total = usages.Count(),
                @q = q
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UsageList() {
            return View();
        }

        [HttpPost]
        public ActionResult UsageList(FormCollection model) {
            _usageDataTable.SetParameters(model);
            var results = _usageDataTable.Execute();
            return Json(results);
        }

        public ActionResult UsageDelete(string keys) {
            if (string.IsNullOrEmpty(keys)) {
                return Json(new MessageResult {
                    IsError = true,
                    Messages = new[] { "Não foi selecionado nenhum item." }
                });
            }
            var idsMap = keys.Split(';');
            foreach (var id in idsMap) {
                var usage = _usageRecords.Table.Where(m => m.AbsEntry == int.Parse(id)).SingleOrDefault();
                if (usage == null) {
                    throw new XNuvemCoreException(string.Format("Chave de usuário {0} não encontrada.", id));
                }
                _usageRecords.Delete(usage);
            }
            return Json(new MessageResult {
                IsError = false,
                Messages = new[] { "Operação concluída com êxito." }
            });
        }
    }
}