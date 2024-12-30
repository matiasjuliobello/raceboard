using Microsoft.Extensions.Primitives;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class DeviceRepository : AbstractRepository, IDeviceRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            //{ "Id", "[Team].Id" },
            //{ "Organization.Id", "[Organization].Id" },
            //{ "Organization.Name", "[Organization].Name" },
            //{ "RaceClass.Id", "[RaceClass].Id" },
            //{ "RaceClass.Name", "[RaceClass].Name"},
            //{ "Championship.Id", "[Championship].Id" },
            //{ "Championship.Name", "[Championship].Name"},
            //{ "Championship.StartDate", "[Championship].StartDate"},
            //{ "Championship.EndDate", "[Championship].EndDate"}
        };

        #endregion

        #region Constructors

        public DeviceRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IDeviceRepository implementation

        public ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal)
        {
            return base.GetTransactionalContext(scope);
        }

        public void ConfirmTransactionalContext(ITransactionalContext context)
        {
            base.ConfirmTransactionalContext(context);
        }

        public void CancelTransactionalContext(ITransactionalContext context)
        {
            base.CancelTransactionalContext(context);
        }

        public bool Exists(string token, ITransactionalContext? context = null)
        {
            string existsQuery = this.GetExistsQuery("[Device]", "Token = @token");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("token", token);

            return this.Execute<bool>(context);
        }

        public Device Get(string token, ITransactionalContext? context = null)
        {
            return this.GetDevice(token, context);
        }

        public int Create(Device device, ITransactionalContext? context = null)
        {
            return this.CreateDevice(device, context);
        }

        public void Update(Device device, ITransactionalContext? context = null)
        {
            this.UpdateDevice(device, context);
        }

        #endregion

        #region Private Methods

        private Device GetDevice(string token, ITransactionalContext? context = null)
        {
            string sql = "SELECT Id, Token FROM [Device]";

            QueryBuilder.AddCommand(sql);
            QueryBuilder.AddCondition("Token = @token");
            QueryBuilder.AddParameter("token", token);

            return base.GetSingleResult<Device>(context);
        }

        private int CreateDevice(Device device, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Device]
                                ( IdPlatform, Token, CreationDate, LastUpdateDate )
                            VALUES
                                ( @idPlatform, @token, @creationDate, @lastUpdateDate )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idPlatform", device.Platform.Id);
            QueryBuilder.AddParameter("token", device.Token);
            QueryBuilder.AddParameter("creationDate", device.CreationDate);
            QueryBuilder.AddParameter("lastUpdateDate", device.LastUpdateDate);
            
            QueryBuilder.AddReturnLastInsertedId();

            int id = base.Execute<int>(context);

            return id;
        }

        private void UpdateDevice(Device device, ITransactionalContext? context = null)
        {
            string sql = @" UPDATE [Device] SET
                                LastUpdateDate = @lastUpdateDate";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("lastUpdateDate", device.LastUpdateDate);
            QueryBuilder.AddParameter("token", device.Token);
            QueryBuilder.AddCondition("Token = @token");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion
    }
}
