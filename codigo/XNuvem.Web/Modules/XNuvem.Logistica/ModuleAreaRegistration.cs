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

namespace XNuvem.Logistica
{
    public class ModuleAreaRegistration : AreaRegistration
    {
        public override string AreaName {
            get {
                return "XNuvem.Logistica";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {

            context.MapRoute(
                name: "Logistica_Default",
                url: "logistic/{controller}/{action}/{id}",
                defaults: new { area = "XNuvem.Logistica", id = UrlParameter.Optional }
            );

            context.Routes.LowercaseUrls = true;
        }
    }
}