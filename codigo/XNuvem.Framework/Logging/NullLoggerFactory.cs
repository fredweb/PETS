/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 *
 * Este código faz parte do Orchard e é livre para distribuição
 * 
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Logging
{
    class NullLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Type type) {
            return NullLogger.Instance;
        }
    }
}
