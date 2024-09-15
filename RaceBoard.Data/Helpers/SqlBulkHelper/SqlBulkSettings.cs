namespace RaceBoard.Data.Helpers.SqlBulkHelper
{
    public class SqlBulkSettings<T>
    {
        public string TableName { get; set; }
        public List<SqlBulkColumnMapping> Mappings { get; set; }
        public List<T> Data { get; set; }

        public SqlBulkSettings()
        {
            this.Mappings = new List<SqlBulkColumnMapping>();
            this.Data = new List<T>();
        }
    }
}