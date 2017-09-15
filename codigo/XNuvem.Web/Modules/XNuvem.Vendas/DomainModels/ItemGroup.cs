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
    public class ItemGroup
    {
        public ItemGroup() {
            Items = new List<ItemLine>();
        }
        public string Name { get; set; }

        public IList<ItemLine> Items {get; set;}
    }
}