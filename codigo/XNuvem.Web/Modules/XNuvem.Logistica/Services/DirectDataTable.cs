/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/


using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XNuvem.UI.DataTable;

namespace XNuvem.Logistica.Services
{
    public class DirectDataTable : IDirectDataTable
    {
        private readonly IDbConnection _connection;
        public string DefaultOrderColumn { get; set; }
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string Search { get; set; }
        public int OrderColumn { get; set; }
        public string OrderDir { get; set; }
        public string ColumnsName { get; set; }
        public string OrderByColumn { get; set; }

        public string SqlText { get; set; }
        public string SqlCount { get; set; }

        public DirectDataTable(IDbConnection connection) {
            _connection = connection;
        }

        public void SetParameters(FormCollection values) {
            Draw = ToInt(values["draw"]);
            Start = ToInt(values["Start"]);
            Length = ToInt(values["length"]);
            Search = values["search[value]"];
            OrderColumn = ToInt(values["order[0][column]"]);
            OrderDir = values["order[0][dir]"];
            OrderByColumn = values[string.Format("columns[{0}][data]", OrderColumn)];
        }

        /// <summary>
        /// Executes an query by Dapper and returns an DataTableResult based on TResult
        /// </summary>
        /// <typeparam name="TResult">Type of the object to return</typeparam>
        /// <param name="sql">Sql query that contains fields for fill TResult</param>
        /// <param name="searchableColumns">Lookup columns</param>
        /// <param name="whereExtension">Extends where for custom serach. Not safe for sql injection</param>
        /// <returns></returns>
        public DataTableResult Execute<TResult>(
            string sql, 
            string[] searchableColumns, 
            string whereExtension = "") {
            var sqlWrapper = BuildSelect(sql, searchableColumns, whereExtension);
            var sqlCountWrapper = BuildCount(sql, whereExtension);

            int count = _connection.ExecuteScalar<int>(sqlCountWrapper);
            int filterCount = count;
            IEnumerable<TResult> data = null;
            if (string.IsNullOrWhiteSpace(Search)) {
                data = _connection.Query<TResult>(sqlWrapper);
            }
            else {
                var filterCountWrapper = BuildFilterCount(sql, searchableColumns, whereExtension);
                filterCount = _connection.ExecuteScalar<int>(filterCountWrapper, new { Search = this.Search + "%" });
                data = _connection.Query<TResult>(sqlWrapper, new { Search = this.Search + "%" });
            }
            
            var result = new DataTableResult();
            result.recordsTotal = count;
            result.recordsFiltered = filterCount;
            result.data = data as IEnumerable<object>;
            return result;
        }

        private int ToInt(string value) {
            int result = 0;
            if (!Int32.TryParse(value, out result)) {
                return 0;
            }
            return result;
        }

        private string BuildSelect(string sql, string[] searchableColumns, string whereExtension) {
            string sqlWrapper = "SELECT * FROM (" + sql + ") _DataTable ";
            string whereClause = "";
            if (!string.IsNullOrEmpty(Search) && searchableColumns.Any()) {
                whereClause += " WHERE (1 != 1 ";
                foreach (var col in searchableColumns) {
                    whereClause += " OR [" + col + "] LIKE @Search ";
                }
                whereClause += " ) ";
            }
            if (!string.IsNullOrWhiteSpace(whereExtension)) {
                if (string.IsNullOrEmpty(whereClause))
                    whereClause += " WHERE " + whereExtension;
                else
                    whereClause += " AND " + whereExtension;
            }
            string orderClause = " ORDER BY ";
            if (!string.IsNullOrEmpty(OrderByColumn)) {
                orderClause += " [" + OrderByColumn + "] " + OrderDir;
            }
            else if (!string.IsNullOrEmpty(DefaultOrderColumn)) {
                orderClause += " [" + DefaultOrderColumn + "] ASC ";
            }
            else {
                orderClause += " 1 ASC ";
            }
            string fetchClause = string.Format( " OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", Start, Length);
            return sqlWrapper + whereClause + orderClause + fetchClause;
        }

        private string BuildCount(string sql, string whereExtension) {
            return "SELECT COUNT(*) FROM (" + sql + ") _DataTable " + (string.IsNullOrWhiteSpace(whereExtension) ? "" : " WHERE ") + whereExtension;
        }

        private string BuildFilterCount(string sql, string[] searchableColumns, string whereExtension) {
            string sqlWrapper = "SELECT COUNT(*) FROM (" + sql + ") _DataTable ";
            string whereClause = "";
            if (!string.IsNullOrEmpty(Search) && searchableColumns.Any()) {
                whereClause += " WHERE (1 != 1 ";
                foreach (var col in searchableColumns) {
                    whereClause += " OR [" + col + "] LIKE @Search ";
                }
                whereClause += " ) ";
            }

            if (!string.IsNullOrWhiteSpace(whereExtension)) {
                if (string.IsNullOrEmpty(whereClause))
                    whereClause += " WHERE " + whereExtension;
                else
                    whereClause += " AND " + whereExtension;
            }

            return sqlWrapper + whereClause;
        }
    }
}