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
using XNuvem.B1;

namespace XNuvem.Vendas.Services
{
    public interface IB1ConnectionPoolFactory
    {
        B1ConnectionPool GetPool();
        IB1Connection GetConnection();
    }
}