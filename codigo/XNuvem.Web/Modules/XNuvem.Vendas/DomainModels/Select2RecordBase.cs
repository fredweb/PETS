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
    /// Select2 plugin needs id and text to work property
    /// </summary>
    public class Select2RecordBase
    {
        public string id { get; set; }

        public string text { get; set; }
    }
}