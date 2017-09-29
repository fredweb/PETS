﻿/****************************************************************************************
 *
 * Autor: George Santos
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using NHibernate;
using System.Data;

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
