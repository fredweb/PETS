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
using XNuvem.Logistica.Services.Models;

namespace XNuvem.Logistica.Services
{
    public interface IB1ConfigurationManager
    {
        CompanySettings GetSettings();
        void StoreSettings(CompanySettings settings);
    }
}