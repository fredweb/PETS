/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace XNuvem.Vendas.Services
{
    public class DefaultDbConnectionFactory : IDbConnectionFactory, IDisposable
    {
        private readonly IB1ConfigurationManager _configurationManager;
        private IDbConnection _connection;
        public DefaultDbConnectionFactory(IB1ConfigurationManager configurationManager) {
            _configurationManager = configurationManager;
        }

        public IDbConnection Create() {
            EnsureConnection();
            return _connection;
        }

        private void EnsureConnection() {
            // TODO: Configurar o IsolationLevel da conexão
            if (_connection != null) {
                if (_connection.State == ConnectionState.Closed) {
                    _connection.Open();
                }
                return;
            }
            var settings = _configurationManager.GetSettings();
            _connection = new SqlConnection(settings.ConnectionString);
            _connection.Open();
        }

        public void Dispose() {
            // Esta conexão é somente para consulta e não tem controle de transação
            if (_connection == null) {
                return;
            }
            if (_connection.State != ConnectionState.Closed) {
                _connection.Close();
            }
            _connection = null;
        }
    }
}