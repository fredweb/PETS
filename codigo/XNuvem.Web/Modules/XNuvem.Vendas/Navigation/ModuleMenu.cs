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

namespace XNuvem.Vendas.Navigation
{
    public class ModuleMenu : IMenuProvider
    {
        public void BuildMenu(MenuBuilder builder) {
            builder.AddGroup("1.3", "Empresas", DefaultClaims.Administrator.Value);
            builder.AddAction("1.3.1", "Configurar empresa", "Edit", "SettingsCompany", DefaultClaims.Administrator.Value, new { area = "XNuvem.Vendas" });
            builder.AddAction("1.3.2", "Utilização", "Usage", "SettingsCompany", DefaultClaims.Administrator.Value, new { area = "XNuvem.Vendas" });
            builder.AddAction("1.3.3", "Lista de utilizações", "UsageList", "SettingsCompany", DefaultClaims.Administrator.Value, new { area = "XNuvem.Vendas" });
            builder.AddGroup("2", "Vendas");
            builder.AddAction("2.1", "Pedido de venda", "Create", "Order", new { area = "XNuvem.Vendas" });
            builder.AddAction("2.2", "Lista de pedidos", "List", "Order", new { area = "XNuvem.Vendas" });
            builder.AddAction("2.3", "Lista de clientes", "List", "Customer", new { area = "XNuvem.Vendas" });
            builder.AddAction("2.4", "Cheques devolvidos", "Cheques", "Customer", new { area = "XNuvem.Vendas" });
            builder.AddAction("2.5", "Contas à receber", "List", "Cobranca", new { area = "XNuvem.Vendas" });
            builder.AddAction("2.6", "Contas à receber - Vencidos", "ListVencidos", "Cobranca", new { area = "XNuvem.Vendas" });
            builder.AddAction("2.7", "Contas à receber - > 60", "List60", "Cobranca", new { area = "XNuvem.Vendas" });
        }
    }
}