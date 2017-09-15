using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XNuvem.Vendas.DomainModels
{
    public class CreditBalance
    {
        #region SQL Queries...
        public const string SqlSelectByMaster = @"
SELECT 
	SUM(_Receivables.DocTotal) DueDocuments,
	SUM(T1.CreditLine) CreditLine,
	SUM(T1.DebtLine) DebtLine,
	SUM(T1.Balance) Balance,
	SUM(T1.OrdersBal) OrdersBal,
	SUM(T1.ChecksBal) ChecksBal	
FROM OCRD T1
LEFT JOIN
(
SELECT DISTINCT 'NFS' DocType, T6.DocEntry RomaneioEntry, T0.DocNum, T0.DocDate, T1.DueDate, T0.CardCode, T0.CardName, T0.SlpCode, T3.SlpName, (T1.INSTOTAL - T1.PAIDTODATE) DocTotal,
            CASE WHEN T6.U_RmnStatus = '5' THEN 'AGUARDANDO PRESTAÇÃO DE CONTAS'
                        WHEN T6.U_RmnStatus = '6' AND T8.DocEntry IS NULL THEN 'DISPONÍVEL PARA PROTOCOLO'
                        WHEN T6.U_RmnStatus = '6' AND T8.DocEntry IS NOT NULL AND DATEDIFF(DAY, T81.U_DocDate, GETDATE()) <= T7.U_RotaCarteira THEN 'COM O VENDEDOR'
                        WHEN T6.U_RmnStatus = '6' AND T8.DocEntry IS NOT NULL AND DATEDIFF(DAY, T81.U_DocDate, GETDATE()) > T7.U_RotaCarteira THEN 'ATRASO COM O VENDEDOR'
                        ELSE TF1.Descr          END AS DocStatus, T4.Details Comments
   FROM OINV T0
   INNER JOIN INV6 T1 ON T0.DOCENTRY = T1.DOCENTRY
   INNER JOIN OCRD T2 ON T0.CARDCODE = T2.CARDCODE
   INNER JOIN OSLP T3 ON T3.SlpCode  = T2.SlpCode
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
   WHERE T1.STATUS = 'O' AND   (T1.INSTOTAL - T1.PAIDTODATE) > 0

UNION ALL

SELECT DISTINCT 'DVS' DocType, NULL RomaneioEntry, T0.DocNum, T0.DocDate, T1.DueDate, T0.CardCode, T0.CardName, T2.SlpCode, T3.SlpName, ((T1.INSTOTAL - T1.PAIDTODATE) * -1) DocTotal,
      ' ' 'STATUS', ' '
   FROM ORIN T0
   INNER JOIN RIN6 T1 ON T0.DOCENTRY = T1.DOCENTRY
   INNER JOIN OCRD T2 ON T0.CARDCODE = T2.CARDCODE
   INNER JOIN OSLP T3 ON T3.SlpCode  = T2.SlpCode
   LEFT  JOIN OCLG T4 ON T4.CardCode = T0.CardCode
                     AND T4.DocNum   = T0.DocNum
                     AND T4.ClgCode  = (SELECT TOP(1) TA.ClgCode FROM OCLG TA WHERE TA.CardCode = T0.CardCode AND TA.DocNum = T0.DocNum ORDER BY 1 DESC)
   LEFT JOIN [@PRV_RM_DROM1] T5 ON T5.U_InvNum = T0.DocEntry
   LEFT JOIN [@PRV_RM_DROM] T6 ON T6.DocEntry = T5.DocEntry
   LEFT JOIN [@PRV_RM_OROTA] T7 ON T7.Code = T6.U_RotaNum
   WHERE T1.STATUS = 'O' AND   (T1.INSTOTAL - T1.PAIDTODATE) > 0

UNION ALL

SELECT DISTINCT 'BOL' AS DocType, TR1.DocEntry RomaneioEntry, T0.BOENUM AS DocNum, T0.PMNTDATE AS DocDate, T0.DueDate, T0.CardCode, T0.CardName,
       T1.SlpCode, T3.SlpName,  T0.BOESUM AS DocTotal,
                        CASE
                        WHEN TR1.U_RmnStatus = '5' THEN
                          'AGUARDANDO PRESTAÇÃO DE CONTAS'
                        WHEN TR1.U_RmnStatus = '6' THEN
                          'DISPONÍVEL PARA PROTOCOLO'  
                        ELSE
                          TF1.Descr      END AS DocStatus, T4.Details Comments
   FROM OBOE T0
   INNER JOIN OCRD T1 ON T0.CARDCODE = T1.CARDCODE
   INNER JOIN OSLP T3 ON T3.SlpCode = T1.SlpCode
   LEFT JOIN ORCT T6 ON T6.DocNum = T0.PmntNum
   LEFT JOIN RCT2 T7 ON T7.DocNum = T6.DocNum AND T7.InvType = 13
   LEFT JOIN OINV T8 ON T8.DocEntry = T7.DocEntry
   LEFT JOIN [@PRV_RM_DROM1] TR0 ON TR0.U_InvNum = T8.DocEntry
   LEFT JOIN [@PRV_RM_DROM] TR1 ON TR1.DocEntry = TR0.DocEntry
   LEFT JOIN CUFD TF0 ON TF0.TableID = '@PRV_RM_DROM' AND TF0.AliasID = 'RmnStatus'
   LEFT JOIN  UFD1 TF1 ON TF1.TableID = TF0.TableID AND TF1.FieldID = TF0.FieldID AND TF1.FldValue = TR1.U_RmnStatus
   LEFT  JOIN OCLG T4 ON T4.CardCode = T0.CardCode                     
                     AND T4.ClgCode  = (SELECT TOP(1) TA.ClgCode FROM OCLG TA WHERE TA.DocType = 24 AND TA.CardCode = T0.CardCode AND TA.DocNum = T0.PmntNum ORDER BY 1 DESC)
   WHERE (T0.BOESTATUS = 'S' OR T0.BOESTATUS = 'G' OR T0.BOESTATUS = 'D')

UNION ALL

SELECT 'CR' DocType, NULL RomaneioEntry, T0.BaseRef DocNum, T0.RefDate DocDate, T1.DueDate, T2.CardCode, T2.CardName, T3.SlpCode,  T3.SlpName, (T1.BalDueDeb - T1.BalDueCred) DocTotal,
       T1.LineMemo DocStatus, '' Comments
   FROM OJDT T0
   INNER JOIN JDT1 T1 ON T0.Transid    = T1.Transid
   INNER JOIN OCRD T2 ON T1.ShortName  = T2.CardCode
   INNER JOIN OSLP T3 ON T3.SlpCode    = T2.SlpCode
   WHERE T1.Account   = '1.1.04.01'
   AND (BalDueDeb - BalDueCred) != 0 
   AND   T0.TransType IN (24, 46)

UNION ALL

SELECT DISTINCT 'LCM' DocType, NULL RomaneioEntry, T0.TransId DocNum, T0.RefDate DocDate, T1.DueDate, T2.CardCode, T2.CardName,  T3.SlpCode, T3.SlpName, (T1.BalDueDeb - T1.BalDueCred) DocTotal,
       T1.LineMemo DocStatus, '' Comments
   FROM OJDT T0
   INNER JOIN JDT1 T1 ON T0.Transid    = T1.Transid
   INNER JOIN OCRD T2 ON T1.ShortName  = T2.CardCode
   INNER JOIN OSLP T3 ON T3.SlpCode    = T2.SlpCode
   WHERE T1.Account   = '1.1.04.01'
   AND   T0.TransType = '30'
   AND   (T1.MthDate IS NULL OR (T1.MthDate IS NOT NULL AND (T1.BalDueDeb - t1.BalDueCred) > 0))

UNION ALL

SELECT DISTINCT  'CHQ' DocType, NULL RomaneioEntry,  T0.CheckNum DocNum, T0.RcptDate DocDate, T0.CheckDate DueDate, T1.CardCode, T1.CardName, T2.SlpCode, T2.SlpName, T0.CheckSum DocTotal,
            CASE
                        WHEN TC1.U_Status IS NULL  THEN
                                   'A COMPENSAR'
                        ELSE
						(SELECT F.Descr FROM CUFD TF INNER JOIN UFD1 F ON F.TableID = TF.TableID AND F.FieldID = TF.FieldID WHERE TF.TableID = '@PRV_CH_OCHH' AND TF.AliasID = 'Status' AND F.FldValue = TC1.U_Status)
            END AS DocStatus, '' Comments
  FROM OCHH T0
  LEFT JOIN OCRD T1 ON T1.CardCode = T0.CardCode
  LEFT JOIN OSLP T2 ON T2.SlpCode = T1.SlpCode
  LEFT JOIN [@PRV_CH_OCHH] TC1  ON TC1.U_CheckKey = T0.CheckKey
  WHERE T0.Deposited = 'N' AND T0.Canceled = 'N'
) 
_Receivables ON _Receivables.CardCode = T1.CardCode AND _Receivables.DueDate < getdate()
WHERE T1.CardCode IN  
	(SELECT CardCode FROM [OCRD] WHERE ISNULL(U_CardCode, CardCode) = 
		(SELECT ISNULL(U_CardCode, CardCode) FROM [OCRD] WHERE CardCode = @CardCode) )
";
        #endregion
        public double DueDocuments { get; set; }
        public double CreditLine { get; set; }
        public double DebtLine { get; set; }
        public double Balance { get; set; }
        public double OrdersBal { get; set; }
        public double ChecksBal { get; set; }
        public bool CreditOverflow {
            get {
                return (Balance + OrdersBal) > CreditLine;
            }
        }
        public bool DebtOverflow {
            get {
                return (Balance + OrdersBal + (-1 * ChecksBal)) > DebtLine;
            }
        }

        public bool Approved {
            get {
                return (DueDocuments <= 0) && !CreditOverflow && !DebtOverflow;
            }
        }
    }
}