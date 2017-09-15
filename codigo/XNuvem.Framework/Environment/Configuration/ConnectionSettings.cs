using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNuvem.Environment.Configuration
{
    [Serializable]
    public class ConnectionSettings : ICloneable
    {
        public ConnectionSettings() {

        }

        public ConnectionSettings(string connectionString, string dataProvider) {
            this.DataConnectionString = connectionString;
            this.DataProvider = dataProvider;
        }

        public string DataConnectionString { get; set; }
        public string DataProvider { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public object Clone() {
            return new ConnectionSettings() {
                DataConnectionString = this.DataConnectionString,
                DataProvider = this.DataProvider,
                CreatedAt = this.CreatedAt,
                UpdatedAt = this.UpdatedAt
            };
        }
    }
}
