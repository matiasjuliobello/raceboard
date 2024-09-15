using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using RaceBoard.Data.Helpers.Interfaces;

namespace RaceBoard.Data.Helpers.SqlBulkHelper
{
    public class SqlBulkInsertHelper : ISqlBulkInsertHelper
    {
        private readonly int _BATCH_SIZE = 1000;
        private readonly int _TIMEOUT = 30;

        private readonly SqlBulkCopyOptions _sqlBulkCopyOptions = SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.CheckConstraints;

        public void PerformBulkInsert<T>(SqlBulkSettings<T> sqlBulkSettings, ITransactionalContext context)
        {
            var connection = context.Transaction.Connection as SqlConnection;
            var transaction = context.Transaction as SqlTransaction;

            var sqlBulkCopy = new SqlBulkCopy(connection, _sqlBulkCopyOptions, transaction);

            var sqlBulkCopyColumnMappings = BuildColumnMappings(sqlBulkSettings.Mappings);

            SetSqlBulkCopyColumnMappings(sqlBulkCopy, sqlBulkSettings.TableName, sqlBulkCopyColumnMappings);
            SetSqlBulkCopyOptions(sqlBulkCopy);

            DataTable dataTable = ConvertIEnumerableToDataTable(sqlBulkSettings.Data);

            sqlBulkCopy.WriteToServer(dataTable);
        }

        public void PerformBulkInsert<T>(IEnumerable<T> data, string tableName, IEnumerable<KeyValuePair<string, string>> columnMappings, ITransactionalContext context)
        {
            var connection = context.Transaction.Connection as SqlConnection;
            var transaction = context.Transaction as SqlTransaction;

            var sqlBulkCopy = new SqlBulkCopy(connection, _sqlBulkCopyOptions, transaction);

            SetSqlBulkCopyColumnMappings(sqlBulkCopy, tableName, columnMappings);
            SetSqlBulkCopyOptions(sqlBulkCopy);

            DataTable dataTable = ConvertIEnumerableToDataTable(data);

            sqlBulkCopy.WriteToServer(dataTable);
        }

        #region Private Methods

        private List<KeyValuePair<string, string>> BuildColumnMappings(List<SqlBulkColumnMapping> sqlBulkColumnMappings)
        {
            var columnMappings = new List<KeyValuePair<string, string>>();

            foreach (var columnMapping in sqlBulkColumnMappings)
            {
                columnMappings.Add(new KeyValuePair<string, string>(columnMapping.PropertyName, columnMapping.ColumnName));
            }

            return columnMappings;
        }

        private void SetSqlBulkCopyColumnMappings(SqlBulkCopy sqlBulkCopy, string tableName, IEnumerable<KeyValuePair<string, string>> columnMappings)
        {
            sqlBulkCopy.DestinationTableName = tableName;

            foreach (var mapping in columnMappings)
            {
                sqlBulkCopy.ColumnMappings.Add(mapping.Key, mapping.Value);
            }
        }

        private void SetSqlBulkCopyOptions(SqlBulkCopy sqlBulkCopy)
        {
            sqlBulkCopy.BatchSize = _BATCH_SIZE;
            sqlBulkCopy.BulkCopyTimeout = _TIMEOUT;
        }

        private DataTable ConvertIEnumerableToDataTable<T>(IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }

        #endregion
    }
}
