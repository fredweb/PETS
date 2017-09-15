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

namespace XNuvem.Vendas.DomainModels
{
    public class BusinessPartnerResume : Select2RecordBase
    {
        #region SQL Queries...
        public static string SqlSelectCompleteBySlpCode(int slpCode) {
            var sql = @"
SELECT * FROM (
SELECT
	T1.CardCode AS id, CONCAT(T1.CardCode, ' - ', T1.CardName) AS [text], 
	T1.CardCode, T1.CardName, T1.CardFName, T1.GroupCode, T1.Balance, T1.DNotesBal, 
	T1.OrdersBal, T1.ChecksBal, T1.Phone1, T1.Phone2, T1.Cellular, T1.Fax, T1.E_Mail, 
    T1.Notes, T1.SlpCode, T1.ValidFor, T1.GroupNum, T4.PymntGroup, T1.PymCode, T1.ListNum, 
	T3.ListName, T1.CreditLine, T1.DebtLine, IIF(ISNULL(T2.TaxId0, '') = '', T2.TaxId4, T2.TaxId0) CpfCnpj, 
	T2.TaxId1 InscricaoEstadual, T1.[Address], T1.ZipCode, T1.City, T1.State1 AS [State], 
	T1.Block, T1.Building, T1.StreetNo
FROM OCRD T1
LEFT JOIN CRD7 T2 ON T2.CardCode = T1.CardCode AND T2.Address = '' AND T2.AddrType = 'S'
LEFT JOIN OPLN T3 ON T3.ListNum = T1.ListNum
LEFT JOIN OCTG T4 ON T4.GroupNum = T1.GroupNum
WHERE (-1 = {0} OR T1.SlpCode = {0}  OR T1.U_SlpCode1 = {0} OR T1.U_SlpCode2 = {0} OR T1.U_SlpCode3 = {0} OR T1.U_SlpCode4 = {0} OR T1.U_SlpCode5 = {0} ) AND CardType = 'C' ) _Query
WHERE (CardCode LIKE @q OR CardName LIKE @q OR CardFName LIKE @q OR CpfCnpj LIKE @q OR InscricaoEstadual LIKE @q)
ORDER BY CardCode OFFSET @start ROWS FETCH NEXT @length ROWS ONLY";
            return string.Format(sql, slpCode);
        }

        public static string SqlCountCompleteBySlpCode(int slpCode) {
            var sql = @"
SELECT COUNT(*) FROM (
SELECT
	T1.CardCode AS id, CONCAT(T1.CardCode, ' - ', T1.CardName) AS [text], 
	T1.CardCode, T1.CardName, T1.CardFName, T1.GroupCode, T1.Balance, T1.DNotesBal, 
	T1.OrdersBal, T1.ChecksBal, T1.Phone1, T1.Phone2, T1.Cellular, T1.Fax, T1.E_Mail, 
    T1.Notes, T1.SlpCode, T1.ValidFor, T1.GroupNum, T4.PymntGroup, T1.PymCode, T1.ListNum, 
	T3.ListName, T1.CreditLine, T1.DebtLine, IIF(ISNULL(T2.TaxId0, '') = '', T2.TaxId4, T2.TaxId0) CpfCnpj, 
	T2.TaxId1 InscricaoEstadual, T1.[Address], T1.ZipCode, T1.City, T1.State1 AS [State], 
	T1.Block, T1.Building, T1.StreetNo
FROM OCRD T1
LEFT JOIN CRD7 T2 ON T2.CardCode = T1.CardCode AND T2.Address = '' AND T2.AddrType = 'S'
LEFT JOIN OPLN T3 ON T3.ListNum = T1.ListNum
LEFT JOIN OCTG T4 ON T4.GroupNum = T1.GroupNum
WHERE (-1 = {0} OR T1.SlpCode = {0} OR T1.U_SlpCode1 = {0} OR T1.U_SlpCode2 = {0} OR T1.U_SlpCode3 = {0} OR T1.U_SlpCode4 = {0} OR T1.U_SlpCode5 = {0} ) AND CardType = 'C' ) _Query
WHERE (CardCode LIKE @q OR CardName LIKE @q OR CardFName LIKE @q OR CpfCnpj LIKE @q OR InscricaoEstadual LIKE @q)
";
            return string.Format(sql, slpCode);
        }

        public static string SqlSelectDataTablesBySlpCode(int slpCode) {
            var sql = @"
SELECT
	T1.CardCode AS id, CONCAT(T1.CardCode, ' - ', T1.CardName) AS [text], 
	T1.CardCode, T1.CardName, T1.CardFName, T1.GroupCode, T1.Balance, T1.DNotesBal, 
	T1.OrdersBal, T1.ChecksBal, T1.Phone1, T1.Phone2, T1.Cellular, T1.Fax, T1.E_Mail, 
    T1.Notes, T1.SlpCode, T1.ValidFor, T1.GroupNum, T4.PymntGroup, T1.PymCode, T1.ListNum, 
	T3.ListName, T1.CreditLine, T1.DebtLine, IIF(ISNULL(T2.TaxId0, '') = '', T2.TaxId4, T2.TaxId0) CpfCnpj, 
	T2.TaxId1 InscricaoEstadual, T1.[Address], T1.ZipCode, T1.City, T1.State1 AS [State], 
	T1.Block, T1.Building, T1.StreetNo
FROM OCRD T1
LEFT JOIN CRD7 T2 ON T2.CardCode = T1.CardCode AND T2.Address = '' AND T2.AddrType = 'S'
LEFT JOIN OPLN T3 ON T3.ListNum = T1.ListNum
LEFT JOIN OCTG T4 ON T4.GroupNum = T1.GroupNum
WHERE CardType = 'C' AND (-1 = {0} OR T1.SlpCode = {0}  OR T1.U_SlpCode1 = {0} OR T1.U_SlpCode2 = {0} OR T1.U_SlpCode3 = {0} OR T1.U_SlpCode4 = {0} OR T1.U_SlpCode5 = {0} )
";
            return String.Format(sql, slpCode);
        }

        public const string SqlSelectByCardCode = @"
SELECT * FROM (
SELECT
	T1.CardCode AS id, CONCAT(T1.CardCode, ' - ', T1.CardName) AS [text], 
	T1.CardCode, T1.CardName, T1.CardFName, T1.GroupCode, T1.Balance, T1.DNotesBal, 
	T1.OrdersBal, T1.ChecksBal, T1.Phone1, T1.Phone2, T1.Cellular, T1.Fax, T1.E_Mail, 
    T1.Notes, T1.SlpCode, T1.ValidFor, T1.GroupNum, T4.PymntGroup, T1.PymCode, T1.ListNum, 
	T3.ListName, T1.CreditLine, T1.DebtLine, IIF(ISNULL(T2.TaxId0, '') = '', T2.TaxId4, T2.TaxId0) CpfCnpj, 
	T2.TaxId1 InscricaoEstadual, T1.[Address], T1.ZipCode, T1.City, T1.State1 AS [State], 
	T1.Block, T1.Building, T1.StreetNo
FROM OCRD T1
LEFT JOIN CRD7 T2 ON T2.CardCode = T1.CardCode AND T2.Address = '' AND T2.AddrType = 'S'
LEFT JOIN OPLN T3 ON T3.ListNum = T1.ListNum
LEFT JOIN OCTG T4 ON T4.GroupNum = T1.GroupNum
WHERE CardType = 'C' ) _Query
WHERE (CardCode = @CardCode)
";

        #endregion

        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CardFName { get; set; }
        /// <summary>
        /// Grupo do parceiro
        /// </summary>
        public int GroupCode { get; set; }
        /// <summary>
        /// Saldo da conta
        /// </summary>
        public double Balance { get; set; }
        /// <summary>
        /// Saldo de entregas de mercadoria
        /// </summary>
        public double DNotesBal { get; set; }
        /// <summary>
        /// Saldo de pedidos
        /// </summary>
        public double OrdersBal { get; set; }
        public double ChecksBal { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Cellular { get; set; }
        public string Fax { get; set; }
        public string E_Mail { get; set; }
        public string Notes { get; set; }
        public int SlpCode { get; set; }
        public string ValidFor { get; set; }
        public int GroupNum { get; set; }
        /// <summary>
        /// Condição de pagamento
        /// Tabela: OCTG
        /// </summary>
        public string PymntGroup { get; set; }
        /// <summary>
        /// Forma de pagamento padrão
        /// </summary>
        public string PymCode { get; set; }
        public int ListNum { get; set; }
        /// <summary>
        /// Nome da lista de preço
        /// Tabela: OPLN
        /// </summary>
        public string ListName { get; set; }
        public double CreditLine { get; set; }
        public double DebtLine { get; set; }
        /// <summary>
        /// CRD7: IdTax0 CNPJ, IdTax4 CPF [Address='' AND AddrType='S']
        /// </summary>
        public string CpfCnpj { get; set; }
        /// <summary>
        /// CRD7: IdTax1 [Address='' AND AddrType='S']
        /// </summary>
        public string InscricaoEstadual { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Block { get; set; }
        public string Building { get; set; }
        public string StreetNo { get; set; }

        #region Computed fields...
        public double LimitBalance {
            get {
                return  DebtLine - TotalDebt;
            }
        }

        public double TotalDebt {
            get {
                return Balance + (-1 * ChecksBal);
            }
        }

        public string FullPhone {
            get { return string.Format("({0}) {1}", Phone2, Phone1); }
        }
        #endregion
    }
}