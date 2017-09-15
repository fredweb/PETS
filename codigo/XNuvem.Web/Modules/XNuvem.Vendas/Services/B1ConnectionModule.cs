/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/


using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNuvem.B1;

namespace XNuvem.Vendas.Services
{
    public class B1ConnectionModule : Module
    {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<DefaultB1ConfigurationManager>().As<IB1ConfigurationManager>().InstancePerDependency();
            builder.RegisterType<DefaultB1ConnectionPoolFactory>().As<IB1ConnectionPoolFactory>().SingleInstance();
            builder.Register((c) => c.Resolve<IB1ConnectionPoolFactory>().GetConnection()).As<IB1Connection>().InstancePerDependency();
            builder.RegisterType<DefaultDbConnectionFactory>().As<IDbConnectionFactory>().InstancePerRequest();
            builder.Register((c) => c.Resolve<IDbConnectionFactory>().Create()).As<IDbConnection>().InstancePerDependency();
            builder.RegisterType<DirectDataTable>().As<IDirectDataTable>().InstancePerDependency();
        }
    }
}
