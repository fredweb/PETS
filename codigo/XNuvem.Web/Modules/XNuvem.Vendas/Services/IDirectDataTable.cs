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
using System.Web.Mvc;
using XNuvem.UI.DataTable;

namespace XNuvem.Vendas.Services
{
    public interface IDirectDataTable
    {
        string DefaultOrderColumn { get; set; }
        int Draw { get; set; }
        int Start { get; set; }
        int Length { get; set; }
        string Search { get; set; }
        int OrderColumn { get; set; }
        string OrderDir { get; set; }
        string ColumnsName { get; set; }
        void SetParameters(FormCollection values);
        DataTableResult Execute<TResult>(string sql, string[] searchableColumns, string whereExtension = "");
    }
}