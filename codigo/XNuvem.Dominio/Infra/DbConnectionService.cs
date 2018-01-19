using System.Data;
using System.Data.SqlClient;

namespace XNuvem.Dominio.Infra
{
    public static class DbConnectionService
    {
        public static IDbConnection GetConnection()
        {
            var csb = new SqlConnectionStringBuilder
            {
                DataSource = "158.69.114.105,52131",
                UserID = "george_usercobrancas",
                Password = "Zyox$093",
                InitialCatalog = "george_dbcobrancas",
                ConnectTimeout = 60
            };
            var connection = new SqlConnection(csb.ConnectionString);
            return connection;
        }
    }
}