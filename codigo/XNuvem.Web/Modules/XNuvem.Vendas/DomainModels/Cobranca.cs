using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XNuvem.Vendas.DomainModels
{
    public class Cobranca
    {
        #region SQL Selects...
        private const string CSSelectBySlp = @"
SELECT
*
FROM
(
	SELECT DISTINCT 
			'NFS' DocType, 
			T0.DocNum DocNum,
			T0.DocDate DocDate, 
			T1.DueDate DueDate, 
			DATEDIFF(DAY, T1.DueDate, GETDATE()) Days,
			T0.CardCode CardCode, 
			T0.CardName CardName, 
			T2.CardFName CardFName,
			T3.SlpName SlpName,
			(T1.INSTOTAL - T1.PAIDTODATE) DocTotal,
			CONCAT('(', T2.Phone2, ') ', T2.Phone1) Phone,
			T2.State1 State, 
			T2.City City,
			CASE WHEN T6.U_RmnStatus = '5' THEN 'AGUARDANDO PRESTAÇÃO DE CONTAS'
					WHEN T6.U_RmnStatus = '6' AND T8.DocEntry IS NULL THEN 'DISPONÍVEL PARA PROTOCOLO'
					WHEN T6.U_RmnStatus = '6' AND T8.DocEntry IS NOT NULL AND DATEDIFF(DAY, T81.U_DocDate, GETDATE()) <= T7.U_RotaCarteira THEN 'COM O VENDEDOR'
					WHEN T6.U_RmnStatus = '6' AND T8.DocEntry IS NOT NULL AND DATEDIFF(DAY, T81.U_DocDate, GETDATE()) > T7.U_RotaCarteira THEN 'ATRASO COM O VENDEDOR'
			ELSE TF1.Descr END AS DocStatus,
            CASE WHEN T6.U_RmnStatus = '5' THEN '1'
					WHEN T6.U_RmnStatus = '6' AND T8.DocEntry IS NULL THEN '2'
					WHEN T6.U_RmnStatus = '6' AND T8.DocEntry IS NOT NULL AND DATEDIFF(DAY, T81.U_DocDate, GETDATE()) <= T7.U_RotaCarteira THEN '3'
					WHEN T6.U_RmnStatus = '6' AND T8.DocEntry IS NOT NULL AND DATEDIFF(DAY, T81.U_DocDate, GETDATE()) > T7.U_RotaCarteira THEN '4'
			ELSE TF1.Descr END AS DocStatusCode,
			T4.Details Comments
	   FROM OINV T0
	   INNER JOIN INV6 T1 ON T0.DOCENTRY = T1.DOCENTRY
	   INNER JOIN OCRD T2 ON T0.CARDCODE = T2.CARDCODE
	   INNER JOIN OSLP T3 ON T3.SlpCode  = T0.SlpCode
	   LEFT  JOIN OCLG T4 ON T4.CardCode = T0.CardCode
						 AND T4.DocNum   = T0.DocNum
						 AND T4.ClgCode  = (SELECT TOP(1) TA.ClgCode FROM OCLG TA WHERE TA.CardCode = T0.CardCode AND TA.DocNum = T0.DocNum ORDER BY 1 DESC)
	   LEFT JOIN [@PRV_RM_DROM1] T5 ON T5.U_InvNum = T0.DocEntry
	   LEFT JOIN [@PRV_RM_DROM] T6 ON T6.DocEntry = T5.DocEntry
	   LEFT JOIN [@PRV_RM_OROTA] T7 ON T7.Code = T6.U_RotaNum
	   LEFT JOIN [@PRV_RM_PRTC1] T8 ON T8.U_InvEntry = T0.DocEntry
	   LEFT JOIN [@PRV_RM_PRTC] T81 ON T81.DocEntry = T8.DocEntry
	   LEFT JOIN CUFD TF0 ON TF0.TableID = '@PRV_RM_DROM' AND TF0.AliasID = 'RmnStatus'
	   LEFT JOIN  UFD1 TF1 ON TF1.TableID = TF0.TableID AND TF1.FieldID = TF0.FieldID AND TF1.FldValue = T6.U_RmnStatus
	   WHERE T1.STATUS = 'O'
				  AND   (T1.INSTOTAL - T1.PAIDTODATE) > 0
	   AND   ({0} = -1 OR T3.SlpCode = {0})

	UNION ALL

	SELECT DISTINCT 
			'DVS' DocType, 
			T0.DocNum DocNum, 
			T0.DocDate DocDate, 
			T1.DueDate DueDate, 
			DATEDIFF(DAY, T1.DueDate, GETDATE()) Days,
			T0.CardCode CardCode, 
			T0.CardName CardName, 
			T2.CardFName CardFName,
			T3.SlpName SlpName, 
			(T1.INSTOTAL - T1.PAIDTODATE) * -1 DocTotal,
			CONCAT('(', T2.Phone2, ') ', T2.Phone1) Phone,
			T2.State1 State, 
			T2.City City,
			' '  DocStatus,
            '-1' DocStatusCode,
			' ' Comments
	   FROM ORIN T0
	   INNER JOIN RIN6 T1 ON T0.DOCENTRY = T1.DOCENTRY
	   INNER JOIN OCRD T2 ON T0.CARDCODE = T2.CARDCODE
	   INNER JOIN OSLP T3 ON T3.SlpCode  = T2.SlpCode
	   LEFT  JOIN OCLG T4 ON T4.CardCode = T0.CardCode
						 AND T4.DocNum   = T0.DocNum
						 AND T4.ClgCode  = (SELECT TOP(1) TA.ClgCode FROM OCLG TA WHERE TA.CardCode = T0.CardCode AND TA.DocNum = T0.DocNum ORDER BY 1 DESC)
	   LEFT JOIN [@PRV_RM_DROM1] T5 ON T5.U_InvNum = T0.DocEntry
	   LEFT JOIN [@PRV_RM_DROM]  T6 ON T6.DocEntry = T5.DocEntry
	   LEFT JOIN [@PRV_RM_OROTA] T7 ON T7.Code = T6.U_RotaNum
	   WHERE T1.STATUS = 'O'
	   AND   (T1.INSTOTAL - T1.PAIDTODATE) > 0
	   AND   ({0} = -1 OR T3.SlpCode = {0})

	UNION ALL

	SELECT DISTINCT 
			'BOL' AS DocType, 
			T0.BOENUM AS DocNum, 
			T0.PMNTDATE AS DocDate, 
			T0.DueDate AS DueDate,
			DATEDIFF(DAY, T0.DueDate, GETDATE()) Days,
			T0.CardCode AS CardCode, 
			T0.CARDNAME AS CardName,
			T1.CardFName AS CardFName, 
			T3.SlpName AS SlpName,  
			T0.BOESUM AS DocTotal,
			CONCAT('(', T1.Phone2, ') ', T1.Phone1) Phone,
			T1.State1 AS State, 
			T1.City AS City,
			CASE WHEN TR1.U_RmnStatus = '5' THEN 'AGUARDANDO PRESTAÇÃO DE CONTAS'
				WHEN TR1.U_RmnStatus = '6' THEN 'DISPONÍVEL PARA PROTOCOLO'
			ELSE TF1.Descr END AS DocStatus,
            '-1' DocStatusCode,
			T4.Details AS Comments
	   FROM OBOE T0
	   INNER JOIN OCRD T1 ON T0.CARDCODE = T1.CARDCODE
	   LEFT JOIN ORCT T6 ON T6.DocNum = T0.PmntNum
	   LEFT JOIN RCT2 T7 ON T7.DocNum = T6.DocNum AND T7.InvType = 13
	   LEFT JOIN OINV T8 ON T8.DocEntry = T7.DocEntry
	   INNER JOIN OSLP T3 ON T3.SlpCode = T8.SlpCode
	   LEFT JOIN [@PRV_RM_DROM1] TR0 ON TR0.U_InvNum = T8.DocEntry
	   LEFT JOIN [@PRV_RM_DROM] TR1 ON TR1.DocEntry = TR0.DocEntry
	   LEFT JOIN CUFD TF0 ON TF0.TableID = '@PRV_RM_DROM' AND TF0.AliasID = 'RmnStatus'
	   LEFT JOIN  UFD1 TF1 ON TF1.TableID = TF0.TableID AND TF1.FieldID = TF0.FieldID AND TF1.FldValue = TR1.U_RmnStatus
	   LEFT  JOIN OCLG T4 ON T4.CardCode = T0.CardCode                     
						 AND T4.ClgCode  = (SELECT TOP(1) TA.ClgCode FROM OCLG TA WHERE TA.DocType = 24 AND TA.CardCode = T0.CardCode AND TA.DocNum = T0.PmntNum ORDER BY 1 DESC)
	   WHERE (T0.BOESTATUS = 'S' OR T0.BOESTATUS = 'G' OR T0.BOESTATUS = 'D')
	   AND   ({0} = -1 OR T3.SlpCode = {0})

	UNION ALL

	SELECT 
			'CR' DocType,
			T0.BaseRef DocNum, 
			T0.RefDate Docate, 
			T1.DueDate DueDate, 
			DATEDIFF(DAY, T1.DueDate, GETDATE()) Days,
			T2.CardCode CardCode, 
			T2.CardName CardName,  
			T2.CardFName CardFName, 
			T3.SlpName SlpName, 
			(T1.BalDueDeb - T1.BalDueCred) DocTotal,	
			CONCAT('(', T2.Phone2, ') ', T2.Phone1) Phone,		
			T2.State1 State, 
			T2.City City,
			' '  DocStatus, 
            '-1' DocStatusCode,
			T1.LineMemo Comments
	   FROM OJDT T0
	   INNER JOIN JDT1 T1 ON T0.Transid    = T1.Transid
	   INNER JOIN OCRD T2 ON T1.ShortName  = T2.CardCode
	   INNER JOIN OSLP T3 ON T3.SlpCode    = T2.SlpCode
	   WHERE T1.Account   = '1.1.04.01'
	   AND (BalDueDeb - BalDueCred) != 0 
	   AND   T0.TransType IN (24, 46)
	   AND   ({0} = -1 OR T3.SlpCode = {0})

	UNION ALL

	SELECT DISTINCT 
			'LCM' DocType, 
			T0.TransId DocNum, 
			T0.RefDate DocDate, 
			T1.DueDate DueDate, 
			DATEDIFF(DAY, T1.DueDate, GETDATE()) Days,
			T2.CardCode CardCode, 
			T2.CardName CardName,  
			T2.CardFName CardFName,
			T3.SlpName SlpName, 
			(T1.BalDueDeb - T1.BalDueCred) DocTotal,
			CONCAT('(', T2.Phone2, ') ', T2.Phone1) Phone,
			T2.State1 State, 
			T2.City City,
			' ' DocStatus, 
            '-1' DocStatusCode,
			T1.LineMemo Comments
	   FROM OJDT T0
	   INNER JOIN JDT1 T1 ON T0.Transid    = T1.Transid
	   INNER JOIN OCRD T2 ON T1.ShortName  = T2.CardCode
	   INNER JOIN OSLP T3 ON T3.SlpCode    = T2.SlpCode
	   WHERE T1.Account   = '1.1.04.01'
	   AND   T0.TransType = '30'
	   AND   (T1.MthDate IS NULL OR (T1.MthDate IS NOT NULL AND (T1.BalDueDeb - t1.BalDueCred) > 0))
	   AND   ({0} = -1 OR T3.SlpCode = {0})

	UNION ALL


	SELECT DISTINCT
		V1.TIPO DocType,
		V1.CheckKey DocNum,
		V1.RcptDate DocDate,
		V1.CheckDate DueDate,
		DATEDIFF(DAY, V1.CheckDate, GETDATE()) Days,
		V1.CardCode CardCode,
		V1.CardName CardName,
		V1.CardFName CardFName,
		V2.SlpName SlpName,
		V1.CheckSum DocTotal,
		CONCAT('(', V1.Phone2, ') ', V1.Phone1) Phone,
		V1.State1 State, 
		V1.City City,
		V1.[STATUS] DocStatus,
        '-1' DocStatusCode,
		CONCAT('BANCO: ',V1.BankCode, ' - AGÊNCIA: ', V1.Branch, ' - CONTA: ', V1.AcctNum, ' - Nº Cheque: ', V1.CheckNum, ' - CONTA DEPÓSITO: ', V1.AcctName) Comments
	FROM 
	(
		SELECT DISTINCT  
			'CHQ' TIPO, 
			T0.CheckKey,
			T0.CheckNum,
			T0.BankCode,
			T0.Branch,
			T0.AcctNum,
			T0.RcptDate, 
			T0.CheckDate, 
			T1.CardCode, 
			T1.CardName, 
			T2.SlpName, 
			T0.CheckSum,
			CASE
				WHEN TC1.U_Status IS NULL  THEN
							'A COMPENSAR'
				ELSE
				(SELECT F.Descr FROM CUFD TF INNER JOIN UFD1 F ON F.TableID = TF.TableID AND F.FieldID = TF.FieldID WHERE TF.TableID = '@PRV_CH_OCHH' AND TF.AliasID = 'Status' AND F.FldValue = TC1.U_Status)
			END AS 'STATUS',
			T6.AcctCode,
			T6.AcctName,
			T1.CardFName, 
			T1.Phone1,
			T1.Phone2,
			T1.State1, 
			T1.City
		FROM OCHH T0
			LEFT JOIN OCRD T1 ON T1.CardCode = T0.CardCode
			LEFT JOIN OSLP T2 ON T2.SlpCode = T1.SlpCode
			LEFT JOIN [@PRV_CH_OCHH] TC1  ON TC1.U_CheckKey = T0.CheckKey
			LEFT JOIN OCHH T5 ON T5.CheckKey = TC1.U_BaseEntry
			LEFT JOIN OACT T6 ON T6.AcctCode = T5.BankAcct
		WHERE T0.Deposited = 'N' AND T0.Canceled = 'N'			
	) V1
	LEFT JOIN (
	SELECT
		T1.CheckKey,
		T1.CheckNum,
		T1.BankCode,
		T1.Branch,
		T1.AcctNum,
		T4.SlpCode,
		T5.SlpName
	FROM 
		OCHH T1
		INNER JOIN RCT1 T2 ON T2.CheckAbs = T1.CheckKey
		INNER JOIN RCT2 T3 ON T3.InvType = 13 AND T3.DocNum = T2.DocNum
		INNER JOIN OINV T4 ON T4.DocEntry = T3.DocEntry
		INNER JOIN OSLP T5 ON T5.SlpCode = T4.SlpCode
	) V2 ON V2.BankCode = V1.BankCode AND V2.Branch = V1.Branch AND V2.AcctNum = V1.AcctNum AND  V2.CheckNum = V1.CheckNum
	WHERE
		({0} = -1 OR V2.SlpCode = {0})
) _T1

";
        public static string SelectBySlpCode(int slpCode) {
            return string.Format(CSSelectBySlp, slpCode);
        }

        public static string SelectVencidosBySlpCode(int slpCode) {
            return SelectBySlpCode(slpCode) + " WHERE DueDate < DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))";
        }

        public static string Select60BySlpCode(int slpCode) {
            return SelectBySlpCode(slpCode) + " WHERE DATEDIFF(DAY, DueDate, GETDATE()) >= 60";
        }

        #endregion

        public string DocType { get; set; }
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DueDate { get; set; }
        public int Days { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CardFName { get; set; }
        public string SlpName { get; set; }
        public double DocTotal { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string DocStatus { get; set; }
        public string DocStatusCode { get; set; }
        public string Comments { get; set; }

        public string DocTotalFmt { get { return DocTotal.ToString("C"); } }
        public string DocDateFmt { get { return DocDate.ToString("dd/MM/yyyy"); } }
        public string DueDateFmt { get { return DueDate.ToString("dd/MM/yyyy"); } }
    }
}