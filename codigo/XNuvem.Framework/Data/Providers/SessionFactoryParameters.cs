/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Data.Providers
{
    public class SessionFactoryParameters : DataServiceParameters
    {
        public SessionFactoryParameters() {
            Configurers = Enumerable.Empty<ISessionConfigurationEvents>();
        }
        public IEnumerable<ISessionConfigurationEvents> Configurers { get; set; }
        public bool CreateDatabase { get; set; }
    }
}
