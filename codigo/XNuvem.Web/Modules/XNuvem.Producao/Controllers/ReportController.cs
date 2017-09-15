using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XNuvem.Data;
using XNuvem.Producao.Models;
using XNuvem.Producao.Records;
using XNuvem.UI;
using Dapper;
using XNuvem.Producao.Services;

namespace XNuvem.Producao.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private const string SqlProducaoWeb = @"
SELECT *
 FROM
(
	SELECT 
		L1.Familia,
		L1.U_Turno Turno,
		L1.Grupo,
		SUM(L1.Quantity) Quantity,
		SUM(L1.TotalVendas) TotalVendas,
		SUM(L1.TotalPacotes) TotalPacotes,
		SUM(L1.TotalKilo) TotalKilo
	FROM
	(
		SELECT
		CASE
				WHEN I.QryGroup1 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 1)
				WHEN I.QryGroup2 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 2)
				WHEN I.QryGroup3 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 3)
				WHEN I.QryGroup4 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 4)
				WHEN I.QryGroup5 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 5)
				WHEN I.QryGroup6 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 6)
			END Familia,
			CASE
				WHEN I.QryGroup21 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 21)
				WHEN I.QryGroup22 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 22)
				WHEN I.QryGroup23 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 23)
				WHEN I.QryGroup24 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 24)
				WHEN I.QryGroup25 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 25)
				WHEN I.QryGroup26 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 26)
				WHEN I.QryGroup27 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 27)
				WHEN I.QryGroup28 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 28)
				WHEN I.QryGroup29 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 29)
				WHEN I.QryGroup30 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 30)
				WHEN I.QryGroup31 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 31)
				WHEN I.QryGroup32 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 32)
				WHEN I.QryGroup33 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 33)
				WHEN I.QryGroup34 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 34)
				WHEN I.QryGroup35 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 35)
				WHEN I.QryGroup36 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 36)
				WHEN I.QryGroup37 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 37)
				WHEN I.QryGroup38 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 38)
			END Grupo,
			V1.Turno,
			V1.U_Turno,
			V1.ItemCode,
			V1.ItemName,
			V1.Quantity,
			(V1.Quantity * I1.Price) TotalVendas,
			(V1.Quantity * I.U_BS_Pacotes) TotalPacotes,
			(I.SWeight1 * V1.Quantity) TotalKilo
		FROM
		(
			SELECT 
				T2.Turno,
				T4.Descr 'U_Turno',
				T1.ItemCode,
				T1.ItemName,
				ISNULL(SUM(T1.Quantity), 0) 'Quantity',
				COUNT(DISTINCT DATEADD(dd, 0, DATEDIFF(dd, 0, T1.CreatedAt))) NumDays
			FROM BSLOCAL..Entradas T1
			LEFT JOIN BSLOCAL..Apontamentos T2 ON T2.DocEntry = T1.Apontamento_DocEntry
			LEFT JOIN (
				SELECT V2.FldValue, V2.Descr  FROM 
				SBO_CAND_GER..CUFD  V1
				LEFT JOIN SBO_CAND_GER..UFD1 V2 ON V2.TableID = V1.TableID AND V2.FieldID = V1.FieldID
				WHERE V1.TableID = 'OIGE' AND V1.AliasID = 'Turno'
			) T4 ON T4.FldValue = T2.Turno COLLATE SQL_Latin1_General_CP850_CI_AS
			LEFT JOIN OITM T5 ON T5.ItemCode = T1.ItemCode COLLATE SQL_Latin1_General_CP850_CI_AS
			WHERE    
				DATEADD(dd, 0, DATEDIFF(dd, 0, T1.CreatedAt))  BETWEEN @StartDate AND @EndDate
			GROUP BY
				T2.Turno,
				T4.Descr,
				T1.ItemCode,
				T1.ItemName
		) V1
		LEFT JOIN OITM I ON I.ItemCode = V1.ItemCode COLLATE SQL_Latin1_General_CP850_CI_AS
		LEFT JOIN ITM1 I1 ON I1.PriceList = 22 AND I1.ItemCode = I.ItemCode
	) L1
	GROUP BY L1.Familia,
		L1.Grupo,
		L1.Turno,
		L1.U_Turno
	) R1
ORDER BY 1, 2, 4, 3";

        private const string SqlMetaProducao = @"
SELECT *
 FROM
(
	SELECT 
		L1.Familia,
		L1.U_Turno Turno,
		L1.Grupo,
		SUM(L1.Quantity) Quantity,
		SUM(L1.TotalCost) TotalCost,
		SUM(L1.TotalVendas) TotalVendas,
		SUM(L1.TotalPacotes) TotalPacotes,
		SUM(L1.TotalKilo) TotalKilo,
		SUM(L1.MetaCPV * L1.Quantity) / SUM(L1.Quantity) MetaCPV,
		CASE 
			WHEN L1.Turno = '2' THEN
				(SELECT M.ManhaPacotes FROM BS_CANDEIAS..MetasProducao M WHERE M.AbsEntry = @Meta)
			WHEN L1.Turno = '3' THEN
				(SELECT M.TardePacotes FROM BS_CANDEIAS..MetasProducao M WHERE M.AbsEntry = @Meta)
			WHEN L1.Turno = '1' THEN
				(SELECT M.NoitePacotes FROM BS_CANDEIAS..MetasProducao M WHERE M.AbsEntry = @Meta)				
		END MetaPacotes,
		CASE 
			WHEN L1.Turno = '2' THEN
				(SELECT M.ManhaVendas FROM BS_CANDEIAS..MetasProducao M WHERE M.AbsEntry = @Meta)
			WHEN L1.Turno = '3' THEN
				(SELECT M.TardeVendas FROM BS_CANDEIAS..MetasProducao M WHERE M.AbsEntry = @Meta)
			WHEN L1.Turno = '1' THEN
				(SELECT M.NoiteVendas FROM BS_CANDEIAS..MetasProducao M WHERE M.AbsEntry = @Meta)				
		END MetaVendas
	FROM
	(
		SELECT
		CASE
				WHEN I.QryGroup1 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 1)
				WHEN I.QryGroup2 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 2)
				WHEN I.QryGroup3 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 3)
				WHEN I.QryGroup4 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 4)
				WHEN I.QryGroup5 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 5)
				WHEN I.QryGroup6 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 6)
			END Familia,
			CASE
				WHEN I.QryGroup21 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 21)
				WHEN I.QryGroup22 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 22)
				WHEN I.QryGroup23 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 23)
				WHEN I.QryGroup24 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 24)
				WHEN I.QryGroup25 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 25)
				WHEN I.QryGroup26 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 26)
				WHEN I.QryGroup27 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 27)
				WHEN I.QryGroup28 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 28)
				WHEN I.QryGroup29 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 29)
				WHEN I.QryGroup30 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 30)
				WHEN I.QryGroup31 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 31)
				WHEN I.QryGroup32 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 32)
				WHEN I.QryGroup33 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 33)
				WHEN I.QryGroup34 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 34)
				WHEN I.QryGroup35 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 35)
				WHEN I.QryGroup36 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 36)
				WHEN I.QryGroup37 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 37)
				WHEN I.QryGroup38 = 'Y' THEN (SELECT TOP 1 ItmsGrpNam FROM OITG WHERE ItmsTypCod = 38)
			END Grupo,
			V1.Turno,
			V1.U_Turno,
			V1.ItemCode,
			V1.ItemName,
			V1.Quantity,
			(V1.Quantity * I.AvgPrice) TotalCost,
			(V1.Quantity * I1.Price) TotalVendas,
			(V1.Quantity * I.U_BS_Pacotes) TotalPacotes,
			(I.SWeight1 * V1.Quantity) TotalKilo,
			(I.U_MetaCPV) MetaCPV
		FROM
		(
			SELECT 
				T2.Turno,
				T4.Descr 'U_Turno',
				T1.ItemCode,
				T1.ItemName,
				ISNULL(SUM(T1.Quantity), 0) 'Quantity',
				COUNT(DISTINCT DATEADD(dd, 0, DATEDIFF(dd, 0, T1.CreatedAt))) NumDays
			FROM BSLOCAL..Entradas T1
			LEFT JOIN BSLOCAL..Apontamentos T2 ON T2.DocEntry = T1.Apontamento_DocEntry
			LEFT JOIN (
				SELECT V2.FldValue, V2.Descr  FROM 
				SBO_CAND_GER..CUFD  V1
				LEFT JOIN SBO_CAND_GER..UFD1 V2 ON V2.TableID = V1.TableID AND V2.FieldID = V1.FieldID
				WHERE V1.TableID = 'OIGE' AND V1.AliasID = 'Turno'
			) T4 ON T4.FldValue = T2.Turno COLLATE SQL_Latin1_General_CP850_CI_AS
			LEFT JOIN OITM T5 ON T5.ItemCode = T1.ItemCode COLLATE SQL_Latin1_General_CP850_CI_AS
			WHERE    
				DATEADD(dd, 0, DATEDIFF(dd, 0, T1.CreatedAt))  BETWEEN @StartDate AND @EndDate
				AND (ISNULL(@Turno, '') = '' OR T4.FldValue = @Turno)
			GROUP BY
				T2.Turno,
				T4.Descr,
				T1.ItemCode,
				T1.ItemName
		) V1
		LEFT JOIN OITM I ON I.ItemCode = V1.ItemCode COLLATE SQL_Latin1_General_CP850_CI_AS
		LEFT JOIN ITM1 I1 ON I1.PriceList = 22 AND I1.ItemCode = I.ItemCode
	) L1
	GROUP BY L1.Familia,
		L1.Grupo,
		L1.Turno,
		L1.U_Turno
	) R1
ORDER BY 1, 2, 4, 3
";

        private readonly IRepository<MetaProducaoRecord> _metasProducao;
        public ReportController(IRepository<MetaProducaoRecord> metasProducao) {
            _metasProducao = metasProducao;
        }
        
        [HttpGet]
        public ActionResult Produtividade()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Produtividade(ProdutividadeViewModel model) {
            using (var connection = DbConnectionService.GetConnection()) {
                connection.Open();

                var pMeta = int.Parse(model.MetaAbs);
                string pTurno = null;
                switch (model.Turno) {
                    case "1":
                        pTurno = "2"; // 2-Manha SAP)
                        break;
                    case "2":
                        pTurno = "3"; // 3 - Tarde SAP
                        break;
                    case "3":
                        pTurno = "1"; // 1 - Noite SAP
                        break;
                }

                model.Items = connection.Query<ProdutividadeItemViewModel>(SqlMetaProducao, new { 
                    @StartDate = model.FromDate, 
                    @EndDate = model.ToDate,
                    @Turno = pTurno,
                    @Meta = pMeta
                }).ToList();
            }

            model.Geral = new ProdutividadeGeralViewModel();
            model.Geral.Quantity = model.Items.Sum(m => m.Quantity);
            model.Geral.TotalCost = model.Items.Sum(m => m.TotalCost);
            model.Geral.TotalKilo = model.Items.Sum(m => m.TotalKilo);
            model.Geral.TotalPacotes = model.Items.Sum(m => m.TotalPacotes);
            model.Geral.TotalVendas = model.Items.Sum(m => m.TotalVendas);

            var familias = model.Items
                .GroupBy(p => p.Familia)
                .Select(p => p.First())
                .Select(p => p.Familia )
                .ToList();

            var turnos = model.Items
                .GroupBy(p => new { p.Familia, p.Turno })
                .Select(p => p.First())
                .ToList();

            model.Familias = new List<ProdutividadeFamiliaItem>();

            foreach (var f in familias) {
                var familiaItem = new ProdutividadeFamiliaItem();
                familiaItem.Familia = f;
                familiaItem.Quantity = model.Items.Where(p => p.Familia == f).Sum(p => p.Quantity);
                familiaItem.TotalCost = model.Items.Where(p => p.Familia == f).Sum(p => p.TotalCost);
                familiaItem.TotalKilo = model.Items.Where(p => p.Familia == f).Sum(p => p.TotalKilo);
                familiaItem.TotalPacotes = model.Items.Where(p => p.Familia == f).Sum(p => p.TotalPacotes);
                familiaItem.TotalVendas = model.Items.Where(p => p.Familia == f).Sum(p => p.TotalVendas);
                familiaItem.MetaPacotes = turnos.Where(p => p.Familia == f).Sum(p => p.MetaPacotes);
                familiaItem.MetaVendas = turnos.Where(p => p.Familia == f).Sum(p => p.MetaVendas);
                familiaItem.Items = new List<ProdutividadeTurnoItem>();
                foreach (var t in turnos.Where(p => p.Familia == f).ToList()) {
                    var turnoItem = new ProdutividadeTurnoItem();
                    turnoItem.Items = model.Items.Where(p => p.Familia == f && p.Turno == t.Turno).ToList();
                    turnoItem.Familia = f;
                    turnoItem.Turno = t.Turno;
                    turnoItem.Quantity = turnoItem.Items.Sum(p => p.Quantity);
                    turnoItem.TotalCost = turnoItem.Items.Sum(p => p.TotalCost);
                    turnoItem.TotalKilo = turnoItem.Items.Sum(p => p.TotalKilo);
                    turnoItem.TotalPacotes = turnoItem.Items.Sum(p => p.TotalPacotes);
                    turnoItem.TotalVendas = turnoItem.Items.Sum(p => p.TotalVendas);
                    turnoItem.MetaPacotes = t.MetaPacotes;
                    turnoItem.MetaVendas = t.MetaVendas;
                    familiaItem.Items.Add(turnoItem);
                }
                model.Familias.Add(familiaItem);
            }
            

            return View(model);
        }

        [HttpGet]
        public ActionResult ProducaoWeb() {
            return View();
        }

        [HttpPost]
        public ActionResult ProducaoWeb(ProdutividadeViewModel model) {
            using (var connection = DbConnectionService.GetConnection()) {
                connection.Open();                

                model.Items = connection.Query<ProdutividadeItemViewModel>(SqlProducaoWeb, new {
                    @StartDate = model.FromDate,
                    @EndDate = model.ToDate
                }).ToList();
            }

            model.Geral = new ProdutividadeGeralViewModel();
            model.Geral.Quantity = model.Items.Sum(m => m.Quantity);
            model.Geral.TotalKilo = model.Items.Sum(m => m.TotalKilo);
            model.Geral.TotalPacotes = model.Items.Sum(m => m.TotalPacotes);
            model.Geral.TotalVendas = model.Items.Sum(m => m.TotalVendas);

            var familias = model.Items
                .GroupBy(p => p.Familia)
                .Select(p => p.First())
                .Select(p => p.Familia)
                .ToList();

            var turnos = model.Items
                .GroupBy(p => new { p.Familia, p.Turno })
                .Select(p => p.First())
                .ToList();

            model.Familias = new List<ProdutividadeFamiliaItem>();

            foreach (var f in familias) {
                var familiaItem = new ProdutividadeFamiliaItem();
                familiaItem.Familia = f;
                familiaItem.Quantity = model.Items.Where(p => p.Familia == f).Sum(p => p.Quantity);
                familiaItem.TotalKilo = model.Items.Where(p => p.Familia == f).Sum(p => p.TotalKilo);
                familiaItem.TotalPacotes = model.Items.Where(p => p.Familia == f).Sum(p => p.TotalPacotes);
                familiaItem.TotalVendas = model.Items.Where(p => p.Familia == f).Sum(p => p.TotalVendas);
                familiaItem.Items = new List<ProdutividadeTurnoItem>();
                foreach (var t in turnos.Where(p => p.Familia == f).ToList()) {
                    var turnoItem = new ProdutividadeTurnoItem();
                    turnoItem.Items = model.Items.Where(p => p.Familia == f && p.Turno == t.Turno).ToList();
                    turnoItem.Familia = f;
                    turnoItem.Turno = t.Turno;
                    turnoItem.Quantity = turnoItem.Items.Sum(p => p.Quantity);
                    turnoItem.TotalKilo = turnoItem.Items.Sum(p => p.TotalKilo);
                    turnoItem.TotalPacotes = turnoItem.Items.Sum(p => p.TotalPacotes);
                    turnoItem.TotalVendas = turnoItem.Items.Sum(p => p.TotalVendas);
                    familiaItem.Items.Add(turnoItem);
                }
                model.Familias.Add(familiaItem);
            }


            return View(model);
        }

        public ActionResult MetaSelect2() {
            var data = new AjaxDropDownResult();
            data.results = _metasProducao.Table.Select(s => new AjaxDropDownResultItem { id = s.AbsEntry.ToString(), text = s.Name }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TurnoSelect2() {
            var data = new AjaxDropDownResult();
            data.results = new AjaxDropDownResultItem[] { 
                new AjaxDropDownResultItem { id = "-1", text = "Todos"},
                new AjaxDropDownResultItem { id = "1", text = "Manhã"},
                new AjaxDropDownResultItem { id = "2", text = "Tarde"},
                new AjaxDropDownResultItem { id = "2", text = "Noite"}
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}