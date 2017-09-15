/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using NHibernate.Cfg;
using System;
using System.Collections;
using System.Collections.Generic;

namespace XNuvem.Data.Providers
{
    public interface ISessionConfigurationCache
    {
        Configuration GetConfiguration();

        void SetConfiguration(Configuration config);

        void InvalidateCache();
    }
}
