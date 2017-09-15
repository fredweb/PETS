using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XNuvem.Vendas.DomainModels
{
    public class OrderDetail
    {
        /// <summary>
        /// Variables: {0} = BusinessSuiteDB, {1} = SAP DB, @iDocEntry, @DocEntry
        /// </summary>
        public const string SqlSelectByDocEntry = @"
SELECT * FROM
(
SELECT
	T0.iDocEntry,
	T1.DocEntry,
	ISNULL(T0.CardCode, T1.CardCode) CardCode,
	ISNULL(T0.CardName, T1.CardName) CardName,
	T0.DocDate CreateDate,
	T1.DocDate OrderDate,
	T2.DocDate DeliveryDate,
	T3.DocDate InvoiceDate,
	T0.DocTotal DocTotal,
	T3.DocTotal InvoiceTotal
FROM
	{0}..Orders T0
	FULL JOIN {1}..ORDR T1 ON T1.DocEntry = T0.DocEntry
	LEFT JOIN {1}..ODLN T2 ON T2.DocEntry = (SELECT TOP 1 P.TrgetEntry FROM RDR1 P WHERE P.DocEntry = T1.DocEntry AND P.TargetType = 15)
	LEFT JOIN {1}..OINV T3 ON 
		T3.DocEntry = (SELECT TOP 1 P.TrgetEntry FROM {1}..RDR1 P WHERE P.DocEntry = T1.DocEntry AND P.TargetType = 13)
		OR T3.DocEntry = (SELECT TOP 1 P.TrgetEntry FROM {1}..DLN1 P WHERE P.DocEntry = T2.DocEntry AND P.TargetType = 13)
) _T0
WHERE
	_T0.iDocEntry = @iDocEntry OR _T0.DocEntry = @DocEntry
";
        public int DocEntry { get; set; }
        public int iDocEntry { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDateFmt { get { return CreateDate != DateTime.MinValue ? CreateDate.ToString("dd/MM/yyyy") : ""; } }
        public DateTime OrderDate { get; set; }
        public string OrderDateFmt { get { return OrderDate != DateTime.MinValue ? OrderDate.ToString("dd/MM/yyyy") : ""; } }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDateFmt { get { return DeliveryDate != DateTime.MinValue ? DeliveryDate.ToString("dd/MM/yyyy") : ""; } }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceDateFmt { get { return InvoiceDate != DateTime.MinValue ? InvoiceDate.ToString("dd/MM/yyyy") : ""; } }
        public double DocTotal { get; set; }
        public string DocTotalFmt { get { return DocTotal.ToString("C"); } }
        public double InvoiceTotal { get; set; }
        public string InvoiceTotalFmt { get { return InvoiceTotal.ToString("C"); } }
    }
}