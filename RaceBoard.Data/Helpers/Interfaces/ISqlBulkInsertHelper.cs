
using RaceBoard.Data.Helpers.SqlBulkHelper;

namespace RaceBoard.Data.Helpers.Interfaces
{
    public interface ISqlBulkInsertHelper
    {
        void PerformBulkInsert<T>(SqlBulkSettings<T> sqlBulkSettings, ITransactionalContext context);
        void PerformBulkInsert<T>(IEnumerable<T> data, string tableName, IEnumerable<KeyValuePair<string, string>> columnMappings, ITransactionalContext context);
    }
}
