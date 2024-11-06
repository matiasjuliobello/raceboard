using System.Data.SqlClient;
using System.Data;
using System.Text;
using static Dapper.SqlMapper;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Constants;
using RaceBoard.Data.Helpers.Interfaces;

namespace RaceBoard.Data.Repositories.Base.Abstract
{
    public abstract class AbstractRepository
    {
        private const int _DEFAULT_TIMEOUT = 30; // seconds
        private string _connectionString;
        private IDbConnection _connection;
        protected IQueryBuilder _queryBuilder;

        protected enum ConditionType
        {
            Equal = 1,
            NotEqual = 2,
            Like = 3,
            NotLike = 4,
            In = 5,
            NotIn = 6,
            GreaterThan = 7,
            GreaterOrEqualThan = 8,
            LessThan = 9,
            LessOrEqualThan = 10
        }

        protected void __FixPaginationResults<T>(ref PaginatedResult<T> items, IEnumerable<T> results, IPaginationFilter? paginationFilter)
        {
            items.Results = results;

            if (paginationFilter != null)
            {
                if (paginationFilter.PageSize == 0)
                    paginationFilter.PageSize = 25;
            }

            items = new PaginatedResult<T>(results.Count(), paginationFilter, items.Results);
        }

        public AbstractRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder)
        {
            _connectionString = contextResolver.GetDatabaseConnection();

            _queryBuilder = queryBuilder;
        }

        public IQueryBuilder QueryBuilder
        {
            get { return _queryBuilder; }
        }

        public ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal)
        {
            var transaction = CreateTransaction();

            return new TransactionalContext(transaction, scope, ConfirmTransaction);
        }

        protected void ConfirmTransactionalContext(ITransactionalContext context)
        {
            if (context == null)
                return;

            if (context.Scope == TransactionContextScope.Internal)
                context.Confirm();
        }

        protected void CancelTransactionalContext(ITransactionalContext context)
        {
            if (context == null)
                return;

            if (context.Scope == TransactionContextScope.Internal)
                context.Cancel();
        }

        #region Query Execution

        protected IDbConnection GetConnection()
        {
            if (_connection == null)
                _connection = new SqlConnection(_connectionString);
            else
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.ConnectionString = _connectionString;
            }

            return _connection;
        }

        protected int ExecuteAndGetRowsAffected(ITransactionalContext? context = null, int? timeout = _DEFAULT_TIMEOUT)
        {
            try
            {
                object affectedRecords = SafeExecute(ExecuteAndGetRowsAffected, context, timeout);

                return (int)affectedRecords;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected T Execute<T>(ITransactionalContext? context = null, int? timeout = _DEFAULT_TIMEOUT)
        {
            var result = SafeExecute(Execute<T>, context, timeout);

            return (T)result;
        }

        protected T GetSingleResult<T>(ITransactionalContext? context = null, int? timeout = _DEFAULT_TIMEOUT)
        {
            var result = SafeExecute(GetSingleResult<T>, context, timeout);

            return (T)result;
        }

        protected IEnumerable<T> GetMultipleResults<T>(ITransactionalContext? context = null, int? timeout = _DEFAULT_TIMEOUT)
        {
            var result = SafeExecute(GetMultipleResults<T>, context, timeout);

            return (IEnumerable<T>)result;
        }
        protected PaginatedResult<T> GetMultipleResultsWithPagination<T>(ITransactionalContext? context = null, int? timeout = _DEFAULT_TIMEOUT)
        {
            var result = SafeExecute(GetPaginatedResultsWithoutMappingFunction<T>, context, timeout);

            return (PaginatedResult<T>)result;
        }

        protected PaginatedResult<T> GetPaginatedResults<T>(Func<GridReader, IEnumerable<T>> mappingFunction, ITransactionalContext? context = null, int? timeout = _DEFAULT_TIMEOUT)
        {
            var result = SafeExecutePaginated(GetPaginatedResults<T>, mappingFunction, context, timeout);

            return (PaginatedResult<T>)result;
        }

        protected void GetReader(Action<GridReader> onReaderExecution, ITransactionalContext? context = null, int? timeout = _DEFAULT_TIMEOUT)
        {
            SafeExecute(GetReader, onReaderExecution, context, timeout);
        }

        #endregion

        #region Sql

        protected string GetIsGreaterOrEqualThanToday(string startDateColumn, string endDateColumn, string? dateToCompareParameterName = null)
        {
            string parameterName = "GETUTCDATE()";
            if (!string.IsNullOrEmpty(dateToCompareParameterName))
                parameterName = dateToCompareParameterName;

            return $"({startDateColumn} <= {parameterName} AND ({endDateColumn} IS NULL OR {endDateColumn} >= {parameterName}))";
        }

        protected string GetExcludeSameRecordCondition(string idColumnName = "Id", string idParametername = "@id")
        {
            return $"IIF({idColumnName} = {idParametername}, {SqlBoolean.False}, {SqlBoolean.True}) = {SqlBoolean.True}";
        }

        protected string GetExistsDuplicateQuery(string tableName, string condition, string idColumnName = "Id", string idParameterName = "@id")
        {
            string duplicateCondition = @$"{GetExcludeSameRecordCondition(idColumnName, idParameterName)} AND ({condition})";

            return GetExistsQuery(tableName, duplicateCondition);
        }

        protected string GetExistsQuery(string tableName, string condition)
        {
            return $"SELECT IIF(EXISTS(SELECT 1 FROM {tableName} WHERE {condition}), {SqlBoolean.True}, {SqlBoolean.False})";
        }

        protected bool Exists(int id, string tableName, string columnName = "Id", ITransactionalContext? context = null)
        {
            string existsQuery = this.GetExistsQuery($"{tableName}", $"{columnName} = @{columnName}");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter(columnName, id);

            return this.Execute<bool>(context);
        }

        protected string GetExistsDatesOverlappingCondition(string startDateColumnName, string endDateColumnName, string startDateParameterName, string endDateParameterName)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"(");
            sb.AppendLine($"       ({startDateColumnName}    BETWEEN {startDateParameterName} AND {endDateParameterName})");
            sb.AppendLine($"    OR ({endDateColumnName}      BETWEEN {startDateParameterName} AND {endDateParameterName})");
            sb.AppendLine($"    OR ({startDateParameterName} BETWEEN {startDateColumnName}    AND {endDateColumnName})");
            sb.AppendLine($"    OR ({endDateParameterName}   BETWEEN {startDateParameterName} AND {endDateColumnName})");
            sb.AppendLine($")");

            return sb.ToString();
        }

        protected string GetBetweenDatesCondition(string compareDate, string startDate, string endDate, bool coalesceStartDate = true, bool coalesceEndDate = true)
        {
            return $"( {GetOnlyDate(compareDate)} BETWEEN {GetOnlyDate(startDate, coalesceStartDate)} AND {GetOnlyDate(endDate, coalesceEndDate)} )";
        }

        protected string GetOnlyDate(string value, bool useMaxValueIfNull = true)
        {
            string date = $"CAST({value} AS DATE)";

            if (useMaxValueIfNull)
                return $"COALESCE({date}, {GetSqlMaxDate()})";
            else
                return date;
        }

        protected string GetCurrentDate()
        {
            return "GETUTCDATE()";
        }

        protected string GetSqlMaxDate()
        {
            return $"'{SqlDate.Max}'";
        }

        protected string CastAsDate(string value)
        {
            return $"CAST({value} AS DATE)";
        }

        protected int Delete(string tableName, int id, string idColumnName = "Id", ITransactionalContext? context = null)
        {
            string sql = $"DELETE FROM {tableName}";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition($"{idColumnName} = @id");
            QueryBuilder.AddParameter("id", id);

            return ExecuteAndGetRowsAffected(context);
        }

        protected int Delete(string tableName, string parameterName, int parameterValue, string condition, ITransactionalContext? context = null)
        {
            string sql = $"DELETE FROM {tableName}";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition(condition);
            QueryBuilder.AddParameter(parameterName, parameterValue);

            return ExecuteAndGetRowsAffected(context);
        }

        protected int Delete(string tableName, int[] ids, string idColumnName = "Id", ITransactionalContext? context = null)
        {
            string sql = $"DELETE FROM {tableName}";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition($"{idColumnName} IN @ids");
            QueryBuilder.AddParameter("ids", ids);

            return ExecuteAndGetRowsAffected(context);
        }

        protected string AddLikeWildcards(string value, bool startsWith = true, bool endsWith = true)
        {
            string wildcard = "'%'";
            string start = startsWith ? $"{wildcard} +" : "";
            string end = endsWith ? $"+ {wildcard}" : "";

            return $"{start} {value} {end}";
        }

        protected void AddFilterCriteria<T>(ConditionType conditionType, string tableName, string columnName, string parameterName, T? value)
        {
            switch (conditionType)
            {
                case ConditionType.Equal:
                    if (value != null && value.GetType() == typeof(bool))
                    {
                        AddEqualsToBooleanCondition(value as bool?, tableName, columnName, parameterName);
                    }
                    if (value != null && value.GetType() == typeof(int))
                    {
                        AddEqualsToIntegerCondition(Convert.ToInt32(value), tableName, columnName, parameterName);
                    }
                    if (value != null && value.GetType() == typeof(decimal))
                    {
                        AddEqualsToDecimalCondition(Convert.ToDecimal(value), tableName, columnName, parameterName);
                    }
                    break;

                case ConditionType.Like:
                    AddLikeCondition(value as string, tableName, columnName, parameterName);
                    break;

                case ConditionType.In:
                    AddWhereInCondition(value as IEnumerable<int>, tableName, columnName, parameterName);
                    break;

                case ConditionType.LessThan:
                    AddComparisonCondition(value, tableName, columnName, "<", parameterName);
                    break;
                case ConditionType.LessOrEqualThan:
                    AddComparisonCondition(value, tableName, columnName, "<=", parameterName);
                    break;
                case ConditionType.GreaterThan:
                    AddComparisonCondition(value, tableName, columnName, ">", parameterName);
                    break;
                case ConditionType.GreaterOrEqualThan:
                    AddComparisonCondition(value, tableName, columnName, ">=", parameterName);
                    break;
            }
        }

        #endregion

        #region Private Methods

        #region Connection & Transaction

        private IDbTransaction CreateTransaction()
        {
            var connection = GetConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var transaction = connection.BeginTransaction();

            return transaction;
        }

        private void ConfirmTransaction(IDbTransaction transaction, bool success)
        {
            if (transaction == null || transaction.Connection == null)
                return;

            if (success)
                transaction.Commit();
            else
                transaction.Rollback();

            CloseConnection(transaction.Connection);
        }

        private void CloseConnection(IDbConnection connection)
        {
            if (connection == null)
                return;

            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        #endregion

        #region Query Execution

        private object SafeExecute<T>(Func<IDbConnection, IDbTransaction?, int?, T> executionCommand, ITransactionalContext? context = null, int? timeout = _DEFAULT_TIMEOUT)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;

            if (context?.Transaction != null)
            {
                if (context.Transaction == null || context.Transaction.Connection == null)
                    throw new FunctionalException(Common.Enums.ErrorType.InternalServerError, "Current Context's Transaction is null.");

                connection = context.Transaction.Connection;
                transaction = context.Transaction;
            }
            else
            {
                connection = GetConnection();
            }

            object result = executionCommand.Invoke(connection, transaction, timeout);

            _queryBuilder.Clear();

            return result;
        }

        private object SafeExecutePaginated<T>(Func<Func<GridReader, IEnumerable<T>>, IDbConnection, IDbTransaction?, int?, PaginatedResult<T>> executionCommand, Func<GridReader, IEnumerable<T>> mappingFunction, ITransactionalContext? context = null, int? timeout = _DEFAULT_TIMEOUT)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;

            if (context?.Transaction != null)
            {
                if (context.Transaction == null || context.Transaction.Connection == null)
                    throw new FunctionalException(Common.Enums.ErrorType.InternalServerError, "Current Context's Transaction is null.");

                connection = context.Transaction.Connection;
                transaction = context.Transaction;
            }
            else
            {
                connection = GetConnection();
            }

            object result = executionCommand.Invoke(mappingFunction, connection, transaction, timeout);

            _queryBuilder.Clear();

            return result;
        }

        private void SafeExecute(Action<Action<GridReader>, IDbConnection, IDbTransaction?, int?> executionCommand, Action<GridReader> onReaderExecution, ITransactionalContext? context = null, int? timeout = _DEFAULT_TIMEOUT)
        {
            IDbConnection connection = null;
            IDbTransaction transaction = null;

            if (context?.Transaction != null)
            {
                if (context.Transaction == null || context.Transaction.Connection == null)
                    throw new FunctionalException(Common.Enums.ErrorType.InternalServerError, "Current Context's Transaction is null.");

                connection = context.Transaction.Connection;
                transaction = context.Transaction;
            }
            else
            {
                connection = GetConnection();
            }

            executionCommand.Invoke(onReaderExecution, connection, transaction, timeout);

            _queryBuilder.Clear();
        }

        private int ExecuteAndGetRowsAffected(IDbConnection connection, IDbTransaction? transaction = null, int? timeout = null)
        {
            return connection.Execute
                (
                    sql: _queryBuilder.Build(),
                    param: _queryBuilder.GetParameters(),
                    transaction: transaction,
                    commandTimeout: timeout
                );
        }

        private T Execute<T>(IDbConnection connection, IDbTransaction? transaction = null, int? timeout = null)
        {
            string sqlQuery = _queryBuilder.Build();
            object parameters = _queryBuilder.GetParameters();

            try
            {
                return connection.ExecuteScalar<T>
                    (
                        sql: sqlQuery,
                        param: parameters,
                        transaction: transaction,
                        commandTimeout: timeout
                    );
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(sqlQuery);
                throw;
            }
        }

        private T GetSingleResult<T>(IDbConnection connection, IDbTransaction? transaction = null, int? timeout = null)
        {
            string sqlQuery = _queryBuilder.Build();
            object parameters = _queryBuilder.GetParameters();

            return connection.QueryFirstOrDefault<T>
                (
                    sql: sqlQuery,
                    param: parameters,
                    transaction: transaction,
                    commandTimeout: timeout
                );
        }

        private IEnumerable<T> GetMultipleResults<T>(IDbConnection connection, IDbTransaction? transaction = null, int? timeout = null)
        {
            string sqlQuery = _queryBuilder.Build();
            object parameters = _queryBuilder.GetParameters();

            return connection.Query<T>
                (
                    sql: sqlQuery,
                    param: parameters,
                    transaction: transaction,
                    commandTimeout: timeout
                );
        }

        private PaginatedResult<T> GetPaginatedResults<T>(Func<GridReader, IEnumerable<T>> mappingFunction, IDbConnection connection, IDbTransaction? transaction = null, int? timeout = null)
        {
            string query = _queryBuilder.Build();
            object parameters = _queryBuilder.GetParameters();
            IPaginationFilter? paginationFilter = _queryBuilder.GetPaginationFilter();

            using (var multi = connection.QueryMultiple(sql: query, param: parameters, transaction: transaction, commandTimeout: timeout))
            {
                int totalRecords = multi.ReadSingle<int>();

                var results = mappingFunction.Invoke(multi);

                return CreatePaginatedResult(results, totalRecords, paginationFilter);
            }
        }

        private PaginatedResult<T> GetPaginatedResultsWithoutMappingFunction<T>(IDbConnection connection, IDbTransaction? transaction = null, int? timeout = null)
        {
            string query = _queryBuilder.Build();
            object parameters = _queryBuilder.GetParameters();
            IPaginationFilter? paginationFilter = _queryBuilder.GetPaginationFilter();

            using (var multi = connection.QueryMultiple(sql: query, param: parameters, transaction: transaction, commandTimeout: timeout))
            {
                int totalRecords = multi.ReadSingle<int>();

                var results = multi.Read<T>();

                return CreatePaginatedResult(results, totalRecords, paginationFilter);
            }
        }

        private PaginatedResult<T> CreatePaginatedResult<T>(IEnumerable<T> items, int totalRecords, IPaginationFilter? paginationFilter)
        {
            return new PaginatedResult<T>(totalRecords, paginationFilter, items);
        }

        private void GetReader(Action<GridReader> action, IDbConnection connection, IDbTransaction? transaction = null, int? timeout = null)
        {
            string sqlQuery = _queryBuilder.Build();
            object parameters = _queryBuilder.GetParameters();

            var reader = connection.QueryMultiple
                (
                    sql: sqlQuery,
                    param: parameters,
                    transaction: transaction,
                    commandTimeout: timeout
                );

            action.Invoke(reader);
        }

        #endregion

        #region Query Building

        private void AddWhereInCondition(IEnumerable<int> values, string tableName, string columnName, string parameterName)
        {
            if (values != null && values.Count() > 0)
            {
                QueryBuilder.AddCondition($"{tableName}.{columnName} IN @{parameterName}");
                QueryBuilder.AddParameter(parameterName, values);
            }
        }
        private void AddEqualsToIntegerCondition(int? value, string tableName, string columnName, string parameterName)
        {
            if (value != null && value > 0)
            {
                QueryBuilder.AddCondition($"{tableName}.{columnName} = @{parameterName}");
                QueryBuilder.AddParameter(parameterName, value);
            }
        }
        private void AddEqualsToDecimalCondition(decimal? value, string tableName, string columnName, string parameterName)
        {
            if (value != null && value > 0)
            {
                QueryBuilder.AddCondition($"{tableName}.{columnName} = @{parameterName}");
                QueryBuilder.AddParameter(parameterName, value);
            }
        }
        private void AddEqualsToBooleanCondition(bool? value, string tableName, string columnName, string parameterName)
        {
            if (value != null)
            {
                QueryBuilder.AddCondition($"{tableName}.{columnName} = @{parameterName}");
                QueryBuilder.AddParameter(parameterName, value.Value);
            }
        }
        private void AddLikeCondition(string? value, string tableName, string columnName, string parameterName)
        {
            if (!string.IsNullOrEmpty(value))
            {
                QueryBuilder.AddCondition($"{tableName}.{columnName} LIKE {AddLikeWildcards($"@{parameterName}")}");
                QueryBuilder.AddParameter(parameterName, value);
            }
        }
        private void AddComparisonCondition<T>(T? value, string tableName, string columnName, string operatorSymbol, string parameterName)
        {
            if (value != null)
            {
                QueryBuilder.AddCondition($"{tableName}.{columnName} {operatorSymbol} @{parameterName}");
                QueryBuilder.AddParameter(parameterName, value);
            }
        }

        #endregion

        #endregion
    }
}