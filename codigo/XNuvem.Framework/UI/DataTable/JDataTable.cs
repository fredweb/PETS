/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XNuvem.Data;
using XNuvem.Linq;
using XNuvem.Logging;

namespace XNuvem.UI.DataTable
{
    public class JDataTable<TEntity> : IJDataTable<TEntity>
    {
        private readonly IRepository<TEntity> _dataSource;
        public ILogger Logger { get; set; }

        public IRepository<TEntity> DataSource {
            get {
                return _dataSource;
            }
        }

        private IList<string> _searchableColumns;

        public string DefaultOrderColumn { get; set; }
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string Search { get; set; }
        public int OrderColumn { get; set; }
        public string OrderDir { get; set; }
        public string ColumnsName { get; set; }
        public string OrderByColumn { get; set; }

        public JDataTable(IRepository<TEntity> dataSource) {
            _searchableColumns = new List<string>();
            _dataSource = dataSource;
            Logger = NullLogger.Instance;
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

        public IJDataTable<TEntity> SearchOn(string propertyName) {
            _searchableColumns.Add(propertyName);
            return this;
        }

        public DataTableResult Execute() {
            return Execute((q) => {
                return q;
            });
        }

        public DataTableResult Execute<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> beforeExecute) {
            var result = new DataTableResult();
            result.draw = Draw;
            var query = DataSource.Table;
                try {
                    // before apply filter
                    result.recordsTotal = query.Count();

                    if (!string.IsNullOrEmpty(Search) && _searchableColumns.Count > 0) {
                        var predicates = PredicateBuilder.False<TEntity>();
                        foreach (var column in _searchableColumns) {
                            string localSearch = Search;
                            predicates = predicates.OrStartsWith(column, localSearch);
                        }
                        query = query.Where(predicates);
                    }
                    var executeQuery = beforeExecute(query);

                    if (!string.IsNullOrEmpty(this.OrderByColumn)) {
                        executeQuery = (this.OrderDir == "asc") ? executeQuery.OrderBy(this.OrderByColumn) : executeQuery.OrderByDescending(this.OrderByColumn);
                    }
                    else if (!string.IsNullOrEmpty(this.DefaultOrderColumn)) {
                        executeQuery = (this.OrderDir == "asc") ? executeQuery.OrderBy(this.DefaultOrderColumn) : executeQuery.OrderByDescending(this.DefaultOrderColumn);
                    }
                    
                    // after apply filter
                    result.recordsFiltered = query.Count();
                    result.data = executeQuery.Skip(Start).Take(Length).ToList() as IEnumerable<object>;
                }
                catch (Exception ex) {
                    Logger.Error(ex, "Erro ao tentar executra a consulta.");
                    result.error = ex.Message;
                }
            
            return result;
        }

        private int ToInt(string value) {
            int result = 0;
            if (!Int32.TryParse(value, out result)) {
                return 0;
            }
            return result;
        }
    }
}
