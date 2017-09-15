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
using XNuvem.Security;
using XNuvem.UI.Navigation;

namespace XNuvem.Logistica.Navigation
{
    public class ModuleMenu : IMenuProvider
    {
        public void BuildMenu(MenuBuilder builder) {
            builder.AddGroup("10", "Logística");
            builder.AddAction("10.1", "Cidades", "Cities", "Maps", new { area = "XNuvem.Logistica" });
            builder.AddAction("10.2", "Rota", "Route", "Maps", new { area = "XNuvem.Logistica" });
            builder.AddGroup("11", "Relatórios");
            builder.AddGroup("11.1", "Vendas");
            builder.AddAction("11.1.1", "Venda por cidade", "Cities", "SalesReport", new { area = "XNuvem.Logistica" });
        }
    }
}