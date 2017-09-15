/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2017  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace XNuvem.Producao
{
    public class ModuleAreaRegistration : AreaRegistration
    {
        public override string AreaName {
            get {
                return "XNuvem.Producao";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {

            context.MapRoute(
                name: "Producao_Default",
                url: "manufac/{controller}/{action}/{id}",
                defaults: new { area = "XNuvem.Producao", id = UrlParameter.Optional }
            );

            context.Routes.LowercaseUrls = true;
        }
    }
}