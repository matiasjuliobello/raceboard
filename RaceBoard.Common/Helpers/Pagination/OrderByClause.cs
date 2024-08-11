namespace RaceBoard.Common.Helpers.Pagination
{
    public class OrderByClause
    {
        private const string _DEFAULT_COLUMN_NAME = "Id";
        private const OrderByDirection _DEFAULT_COLUMN_DIRECTION = OrderByDirection.Ascending;

        public string ColumnName { get; set; }
        public OrderByDirection? Direction { get; set; }

        private OrderByClause()
        {
        }

        public OrderByClause(string columnName, OrderByDirection direction)
        {
            this.ColumnName = columnName;
            this.Direction = direction;
        }

        public static OrderByClause Default
        {
            get
            {
                return new OrderByClause()
                {
                    ColumnName = _DEFAULT_COLUMN_NAME,
                    Direction = _DEFAULT_COLUMN_DIRECTION
                };
            }
        }
    }
}
