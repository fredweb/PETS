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
using System.Web.Routing;
using XNuvem.Mvc;

namespace XNuvem.Vendas
{
    public class ModuleAreaRegistration : AreaRegistration
    {
        public override string AreaName {
            get {
                return "XNuvem.Vendas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {

            context.MapRoute(
                name: "Sales_Default",
                url: "sales/{controller}/{action}/{id}",
                defaults: new { area = "XNuvem.Vendas", id = UrlParameter.Optional }
            );

            context.Routes.LowercaseUrls = true;
        }
    }
}