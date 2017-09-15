/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Data
{
    public interface ITransactionManager : IDependency
    {
        void Demand();
        void RequireNew();
        void RequireNew(IsolationLevel level);
        void Cancel();

        ISession GetSession();
    }
}
