namespace RaceBoard.Data.Helpers.SqlBulkHelper
{
    public class SqlBulkColumnMapping
    {
        public string PropertyName { get; set; }
        public string ColumnName { get; set; }

        public SqlBulkColumnMapping(string propertyName, string columnName)
        {
            PropertyName = propertyName;
            ColumnName = columnName;
        }
    }
}
