using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class DeviceSubscriptionRepository : AbstractRepository, IDeviceSubscriptionRepository
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

        public DeviceSubscriptionRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IDeviceSubscriptionRepository implementation

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

        public bool Exists(DeviceSubscription deviceSubscription, ITransactionalContext? context = null)
        {
            string existsQuery = this.GetExistsQuery("[Device_Subscription]", "IdDevice = @idDevice");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("idDevice", deviceSubscription.Device.Id);

            return this.Execute<bool>(context);
        }

        public DeviceSubscription Get(int idDevice, ITransactionalContext? context = null)
        {
            return this.GetDeviceSubscription(idDevice, context);
        }

        public void Create(DeviceSubscription userDeviceSubscription, ITransactionalContext? context = null)
        {
            this.CreateDeviceSubscription(userDeviceSubscription, context);
        }

        public void Update(DeviceSubscription userDeviceSubscription, ITransactionalContext? context = null)
        {
            throw new NotImplementedException();
        }

        public int Remove(int idDevice, ITransactionalContext? context = null)
        {
            return this.RemoveDeviceSubscription(idDevice, context);
        }

        #endregion

        #region Private Methods

        private DeviceSubscription GetDeviceSubscription(int idDevice, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Device_Subscription].Id [Id],
                                [Device].Id [Id],
                                [Championship].Id [Id],
                                [Championship].Name [Name],
                                [City].Id [Id],
                                [City].Name [Name],
                                [Country].Id [Id],
                                [Country].Name [Name],
                                [RaceClass].Id [Id],
                                [RaceClass].Name [Name]
                            FROM [Device_Subscription]
                            INNER JOIN [Device] ON [Device].Id = [Device_Subscription].IdDevice
                            INNER JOIN [Championship] ON [Championship].Id = [Device_Subscription].IdChampionship
                            INNER JOIN [City] [City] ON [City].Id = [Championship].IdCity
                            INNER JOIN [Country] [Country] ON [Country].Id = [City].IdCountry
                            INNER JOIN [RaceClass] ON [RaceClass].Id = [Device_Subscription].IdRaceClass";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddCondition("IdDevice = @idDevice");
            QueryBuilder.AddParameter("idDevice", idDevice);

            var subscriptions = new List<DeviceSubscription>();

            base.GetReader
                (
                    (x) =>
                    {
                        subscriptions = x.Read<DeviceSubscription, Device, Championship, City, Country, RaceClass, DeviceSubscription>
                        (
                            (subscription, device, championship, city, country, raceClass) =>
                            {
                                var item = subscriptions.FirstOrDefault(x => x.Device.Id == device.Id);
                                if (item == null)
                                {
                                    item = subscription;
                                    item.Device = device;
                                    item.Championship = championship;
                                    subscriptions.Add(item);
                                }
                                item.RaceClasses.Add(raceClass);

                                city.Country = country;
                                championship.City = city;

                                return subscription;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        ).ToList();
                    },
                    context
                );

            return subscriptions.FirstOrDefault();
        }

        private void CreateDeviceSubscription(DeviceSubscription deviceSubscription, ITransactionalContext? context = null)
        {
            var ids = new List<int>();

            foreach (var raceClass in deviceSubscription.RaceClasses)
            {
                string sql = @" INSERT INTO [Device_Subscription]
                                    ( IdDevice, IdChampionship, IdRaceClass )
                                VALUES
                                    ( @idDevice, @idChampionship, @idRaceClass )";

                QueryBuilder.AddCommand(sql);

                QueryBuilder.AddParameter("idDevice", deviceSubscription.Device.Id);
                QueryBuilder.AddParameter("idChampionship", deviceSubscription.Championship.Id);
                QueryBuilder.AddParameter("idRaceClass", raceClass.Id);

                QueryBuilder.AddReturnLastInsertedId();

                int id = base.Execute<int>(context);
                ids.Add(id);
            }
        }

        private int RemoveDeviceSubscription(int idDevice, ITransactionalContext? context = null)
        {
            return base.Delete("Device_Subscription", "idDevice", idDevice, "IdDevice = @idDevice", context);
        }

        #endregion
    }
}
