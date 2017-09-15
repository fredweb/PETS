/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Data.Providers
{
    public interface IDataServiceProvider : ITransientDependency
    {
        Configuration BuildConfiguration(SessionFactoryParameters sessionFactoryParameters);
        IPersistenceConfigurer GetPersistenceConfigurer(SessionFactoryParameters sessionFactoryParameters);
    }
}
