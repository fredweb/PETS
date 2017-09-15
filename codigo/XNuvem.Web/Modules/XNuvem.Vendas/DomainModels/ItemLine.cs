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
    public class ItemLine
    {
        /// <summary>
        /// Query command to select all items by parameter @PriceList
        /// </summary>
        public const string SelectByPriceList = @"
SELECT 
	T0.U_BS_Group AS GroupName, T0.ItemCode, T0.ItemName, T2.ListName, T1.Price, T0.OnHand, T0.OnOrder, 0 AS Quantity, T0.U_BS_Color AS Color
FROM 
	OITM T0
	INNER JOIN ITM1 T1 ON T1.ItemCode = T0.ItemCode and PriceList = @PriceList
	LEFT JOIN OPLN T2 ON T2.ListNum = T1.PriceList
WHERE 
	T0.SellItem = 'Y' AND T0.validFor = 'Y'
	AND ( (@Qry60 ='Y' AND T0.QryGroup60 = 'Y') OR (@Qry61 = 'Y' AND T0.QryGroup61 = 'Y') OR (@Qry62 = 'Y' AND T0.QryGroup62 = 'Y') )
ORDER BY 1 ASC, 2 ASC
";

        public const string SelectByPriceListOrder = @"
SELECT 
	T0.U_BS_Group AS GroupName, T0.ItemCode, T0.ItemName, T2.ListName, T1.Price, T0.OnHand, T0.OnOrder, L.Quantity, T0.U_BS_Color AS Color
FROM 
	OITM T0
	INNER JOIN ITM1 T1 ON T1.ItemCode = T0.ItemCode and PriceList = @PriceList
	LEFT JOIN OPLN T2 ON T2.ListNum = T1.PriceList
	FULL JOIN [{0}]..[OrderLines] L ON L.ItemCode = T0.ItemCode AND L.iDocEntry = @iDocEntry
WHERE 
    T0.SellItem = 'Y' AND T0.validFor = 'Y'
    AND ( (@Qry60 ='Y' AND T0.QryGroup60 = 'Y') OR (@Qry61 = 'Y' AND T0.QryGroup61 = 'Y') OR (@Qry62 = 'Y' AND T0.QryGroup62 = 'Y') )
ORDER BY 1 ASC, 2 ASC
";

        /// <summary>
        /// Grupo do item
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// Código do item
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// Nome do item
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// Nome da lista de preço do item
        /// </summary>
        public string ListName { get; set; }
        /// <summary>
        /// Preço atual referente a lista de preço
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Em estoque físico atualmente
        /// </summary>
        public double OnHand { get; set; }
        /// <summary>
        /// Total aberto  em pedidos
        /// </summary>
        public double OnOrder { get; set; }

        public double Quantity { get; set; }

        public string Color { get; set; }
    }
}