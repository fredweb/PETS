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
using XNuvem.Logging;
using XNuvem.Vendas.Services.Models;

namespace XNuvem.Vendas.Services
{
    public class DefaultB1ConnectionPoolFactory : IB1ConnectionPoolFactory
    {
        private CompanySettings _companySettings;
        private B1ConnectionPool _pool;
        private readonly IB1ConfigurationManager _configurationManager;

        public ILogger Logger { get; set; }
        public DefaultB1ConnectionPoolFactory(IB1ConfigurationManager configurationManager) {
            _configurationManager = configurationManager;
            Logger = NullLogger.Instance;
        }

        private void EnsurePool() {
            var settings = _configurationManager.GetSettings();
            if (settings.Equals(_companySettings) && _pool != null)
                return;

            if (_pool != null) {
                Logger.Debug("Disposing previous Pool due settings modification.");
                (_pool as IDisposable).Dispose();
            }
                
            // case is the same settings return
            
            Logger.Debug("Creating SAP connection pool on server {0}.", settings.ServerName);
            _companySettings = settings;
            var connectionParams = new B1ConnectionParams {
                Server = settings.ServerName,
                CompanyDB = settings.CompanyDB,
                UserName = settings.UserName,
                Password = settings.Password
            };
            int minPool, maxPool;
            if (int.TryParse(settings.MinPoolSize, out minPool) && minPool > 0)
                connectionParams.MinPoolSize = minPool;
            if (int.TryParse(settings.MaxPoolSize, out maxPool) && maxPool > 1)
                connectionParams.MaxPoolSize = maxPool;

            _pool = new B1ConnectionPool(connectionParams);
            Logger.Debug(
                "Done creating SAP connection pool. Server: {0}; CompanyDB: {1}; User: {2}; MinPoolSize: {3}", 
                settings.ServerName, 
                settings.CompanyDB, 
                settings.UserName,
                settings.MinPoolSize);
        }

        public B1ConnectionPool GetPool() {
            EnsurePool();
            return _pool;
        }

        public IB1Connection GetConnection() {
            var pool = GetPool();
            return pool.GetObject();
        }
    }
}