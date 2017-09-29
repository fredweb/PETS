/****************************************************************************************
 *
 * Autor: George Santos
 * Copyright (c) 2016  
 *
 * Este código faz parte do Orchard e é livre para distribuição
 * 
 * 
/****************************************************************************************/

using System;

namespace XNuvem.Logging
{
    public class NullLogger : ILogger
    {
        private static readonly ILogger _instance = new NullLogger();

        public static ILogger Instance {
            get { return _instance; }
        }

        public bool IsEnabled(LogLevel level) {
            return false;
        }

        public void Log(LogLevel level, Exception exception, string format, params object[] args) {
        }
    }
}
