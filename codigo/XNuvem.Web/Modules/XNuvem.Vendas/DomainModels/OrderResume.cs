using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XNuvem.Vendas.DomainModels
{
    public class OrderResume
    {      

        public static string GetSqlDirectTable(string bsDatabaseName) {
            var sql = @"
-- Only orders from SAP
SELECT  0 AS iDocEntry, T1.DocEntry, T1.DocDate, T1.CardCode, 
	T1.CardName, T1.GroupNum, T2.PymntGroup, T1.PeyMethod,
	T1.SlpCode, T3.SlpName, T1.CANCELED AS Canceled,
	T1.DocStatus, T1.DocTotal, 1 AS Approved, T1.U_WB_RouteNumber AS RotaCode
FROM 
	ORDR T1
	LEFT JOIN OCTG T2 ON T2.GroupNum = T1.GroupNum
	LEFT JOIN OSLP T3 ON T3.SlpCode = T1.SlpCode
WHERE NOT EXISTS (SELECT 1 FROM [{0}]..[Orders] WHERE DocEntry = T1.DocEntry)
-- Orders from SAP that was in Business Suite
UNION ALL SELECT 
	T4.iDocEntry, T1.DocEntry, T1.DocDate,
	T1.CardCode, T1.CardName, T1.GroupNum,
	T2.PymntGroup, T1.PeyMethod, T1.SlpCode,
	T3.SlpName, T1.CANCELED AS Canceled,
	T1.DocStatus, T1.DocTotal, T4.Approved, T1.U_WB_RouteNumber AS RotaCode
FROM
	[{0}]..[Orders] T4 
	INNER JOIN ORDR T1 ON T1.DocEntry = T4.DocEntry
	LEFT JOIN OCTG T2 ON T2.GroupNum = T1.GroupNum
	LEFT JOIN OSLP T3 ON T3.SlpCode = T1.SlpCode
WHERE T4.InSbo = 1
-- Orders from Business Suite only
UNION ALL SELECT 
	T1.iDocEntry, T1.DocEntry, T1.DocDate,
	T1.CardCode, T1.CardName, T1.GroupNum,
	T1.PymntGroup, T1.PeyMethod, T1.SlpCode,
	T2.SlpName, T1.Canceled, T1.DocStatus,
	T1.DocTotal, T1.Approved, T1.RotaCode
FROM
	[{0}]..[Orders] T1
	LEFT JOIN OSLP T2 ON T2.SlpCode = T1.SlpCode
WHERE T1.InSbo = 0
";
            return string.Format(sql, bsDatabaseName);
        }

        public int iDocEntry { get; set; }
        public int DocEntry { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int GroupNum { get; set; }
        public string PymntGroup { get; set; }
        public string PeyMethod { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public string Canceled { get; set; }
        public string DocStatus { get; set; }
        public double DocTotal { get; set; }
        public bool Approved { get; set; }
        public string RotaCode { get; set; }
        public string DocDateString {
            get {
                return DocDate.ToString("dd/MM/yyyy");
            }
        }
    }
}