using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XNuvem.Data;
using XNuvem.Producao.Models;
using XNuvem.Producao.Records;
using XNuvem.UI.DataTable;
using XNuvem.UI.Model;

namespace XNuvem.Producao.Controllers
{
    [Authorize]
    public class ConfigController : Controller
    {
        private IJDataTable<MetaProducaoRecord> _jdtMetaProducao;
        private IRepository<MetaProducaoRecord> _metaProducaoRepository;

        public ConfigController(
            IJDataTable<MetaProducaoRecord> jdtMetaProducao,
            IRepository<MetaProducaoRecord> metaProducaoRepository) {
            _jdtMetaProducao = jdtMetaProducao;
            _metaProducaoRepository = metaProducaoRepository;
        }

        [HttpGet]
        public ActionResult MetaProducaoList() {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> MetaProducaoList(FormCollection values) {
            _jdtMetaProducao.SetParameters(values);
            var result = await Task.Run<DataTableResult>(() => _jdtMetaProducao.Execute());
            return Json(result);
        }

        [HttpGet]
        public ActionResult MetaProducaoEdit(int? id) {
            if (id.HasValue) {
                var record = _metaProducaoRepository.Get(id.Value);
                var model = new MetaProducaoViewModel() {
                    AbsEntry = record.AbsEntry,
                    Name = record.Name,
                    ManhaPacotes = record.ManhaPacotes,
                    ManhaVendas = record.ManhaVendas,
                    TardePacotes = record.TardePacotes,
                    TardeVendas = record.TardeVendas,
                    NoitePacotes = record.NoitePacotes,
                    NoiteVendas = record.NoiteVendas
                };
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public ActionResult MetaProducaoEdit(MetaProducaoViewModel model) {
            //TODO: Adicionar os dados ou editar
            var record = new MetaProducaoRecord();
            record.AbsEntry = model.AbsEntry ?? 0;
            record.Name = model.Name;
            record.ManhaPacotes = model.ManhaPacotes;
            record.ManhaVendas = model.ManhaVendas;
            record.TardePacotes = model.TardePacotes;
            record.TardeVendas = model.TardeVendas;
            record.NoitePacotes = model.NoitePacotes;
            record.NoiteVendas = model.NoiteVendas;

            if (record.AbsEntry == 0)
                _metaProducaoRepository.Create(record);
            else
                _metaProducaoRepository.Update(record);
            
            return Json(new MessageResult {
                IsError = false,
                Messages = new[] { "Operação concluída com êxito." }
            });
        }

        public ActionResult MetaProducaoDelete(string keys) {
            if (string.IsNullOrEmpty(keys)) {
                return Json(new MessageResult {
                    IsError = true,
                    Messages = new[] { "Não foi selecionado nenhum item." }
                });
            }
            var idsMap = keys.Split(';');
            foreach (var id in idsMap) {
                var record = _metaProducaoRepository.Table.Where(m => m.AbsEntry == int.Parse(id)).SingleOrDefault();
                if (record == null) {
                    throw new XNuvemCoreException(string.Format("Meta {0} não encontrada.", id));
                }
                _metaProducaoRepository.Delete(record);
            }
            return Json(new MessageResult {
                IsError = false,
                Messages = new[] { "Operação concluída com êxito." }
            });
        }
    }
}