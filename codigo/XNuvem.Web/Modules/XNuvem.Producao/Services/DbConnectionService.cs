using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace XNuvem.Producao.Services
{
    public static class DbConnectionService
    {
        public static IDbConnection GetConnection() {
            var csb = new SqlConnectionStringBuilder();

            //csb.DataSource = "M3M-PC";
            //csb.UserID = "sa";
            //csb.Password = "sysmaker";

            csb.DataSource = "192.168.137.2";
            csb.UserID = "sa";
            csb.Password = "5cd998b744c8.2012";

            csb.InitialCatalog = "SBO_CAND_GER";
            var connection = new SqlConnection(csb.ConnectionString);
            return connection;
        }
    }
}