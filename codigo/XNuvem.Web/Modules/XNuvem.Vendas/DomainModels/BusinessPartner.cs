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
    /// <summary>
    /// TABELA: OCRD
    /// </summary>
    public class BusinessPartner
    {
        public const string SqlSelectByCardCode = @"
SELECT
	T1.CardCode, T1.CardName, T1.CardFName, T1.GroupCode, T2.GroupName, T1.Balance, T1.DNotesBal,
	T1.OrdersBal, (-1 * T1.ChecksBal) AS ChecksBal, T1.Phone1, T1.Phone2, T1.Cellular, T1.Fax, T1.E_Mail, T1.Notes,
	T1.SlpCode, T3.SlpName, T1.validFor, T1.GroupNum, T4.PymntGroup, T1.PymCode, T1.ListNum, T5.ListName,
	T1.CreditLine, T1.DebtLine, IIF(ISNULL(T6.TaxId0, '') = '', TaxId4, TaxId0) Cnpj, T6.TaxId1 InscricaoEstadual
FROM 
	OCRD T1 
	LEFT JOIN OCRG T2 ON T2.GroupCode = T1.GroupCode
	LEFT JOIN OSLP T3 ON T3.SlpCode = T1.SlpCode
	LEFT JOIN OCTG T4 ON T4.GroupNum = T1.GroupNum
	LEFT JOIN OPLN T5 ON T5.ListNum = T1.ListNum
	LEFT JOIN CRD7 T6 ON T6.CardCode = T1.CardCode AND T6.Address = '' AND T6.AddrType = 'S'
WHERE T1.CardCode = @CardCode
";
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CardFName { get; set; }
        /// <summary>
        /// Grupo do parceiro
        /// </summary>
        public int GroupCode { get; set; }
        /// <summary>
        /// Nome do grupo do parceiro
        /// OCRG.GroupName
        /// </summary>
        public string GroupName { get; set; }
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
        public string SlpName { get; set; }
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
        /// <summary>
        /// Table: CRD1 - Addresses
        /// </summary>
        public IEnumerable<BusinessPartnerAddress> Address { get; set; }
        /// <summary>
        /// Table CRD2 - Pay Methods
        /// </summary>
        public IEnumerable<BusinessPartnerPayMethod> PayMethods { get; set; }
    }

    /// <summary>
    /// Tabela: CRD1
    /// </summary>
    public class BusinessPartnerAddress
    {
        public const string SqlSelectByCardCode = @"
SELECT T1.CardCode, T1.Address, T1.AddrType, T1.Street, T1.StreetNo, T1.Building, T1.Block, T1.City, 
	T1.State, T1.County, T2.Name CountyName
FROM 
	CRD1 T1
	LEFT JOIN OCNT T2 ON T2.Code = T1.County
WHERE CardCode = @CardCode
";
        public string CardCode { get; set; }
        public string Address { get; set; }
        public string AddrType { get; set; }
        public string Street { get; set; }
        public string StreetNo { get; set; }
        /// <summary>
        /// Complemento
        /// </summary>
        public string Building { get; set; }
        /// <summary>
        /// Bairro
        /// </summary>
        public string Block { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        /// <summary>
        /// Município codificado
        /// tabela: OCNT
        /// </summary>
        public string County { get; set; }
        /// <summary>
        /// Nome do município codificado
        /// tabela: OCNT
        /// </summary>
        public string CountyName { get; set; }
    }

    public class BusinessPartnerPayMethod
    {
        public const string SqlSelectByCardCode = "SELECT CardCode, LineNum, PymCode FROM CRD2 WHERE CardCode = @CardCode";
        public string CardCode { get; set; }
        public int LineNum { get; set; }
        public string PymCode { get; set; }
    }
}