/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace XNuvem.Logistica.Services
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }
}