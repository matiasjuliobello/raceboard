using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System.Data;

namespace RaceBoard.Data.Repositories
{
    public class MemberRepository : AbstractRepository, IMemberRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Championship].Id" },
            { "Name", "[Championship].Name" },
            { "Organization.Id", "[Organization].Id" },
            { "Organization.Name", "[Organization].Name" },
            { "Team.Id", "[Team].Id" },
            { "RaceClass.Id", "[RaceClass].Id" },
            { "RaceClass.Id", "[RaceClass].Name" },
            { "User.Id", "[User].Id" },
            { "Person.Id", "[Person].Id" },
            { "Person.Firstname", "[Person].Firstname" },
            { "Person.Lastname", "[Person].Lastname" }
        };

        #endregion

        #region Constructors

        public MemberRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IMemberRepository implementation

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

        public PaginatedResult<Member> Get(MemberSearchFilter memberSearchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetMembers(memberSearchFilter, paginationFilter, sorting, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Member> GetMembers(MemberSearchFilter memberSearchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Championship].Id [Id],         
                                [Championship].Name [Name],
                                [Organization].Id [Id],
	                            [Organization].Name [Name],
	                            [Team].Id [Id],
	                            [RaceClass].Id [Id],
	                            [RaceClass].Name [Name],
	                            [User].Id [Id],
	                            [User].Email [Email],
	                            [Person].Id [Id],
	                            [Person].Firstname [Firstname],
	                            [Person].Lastname [Lastname]
                            FROM [Championship] [Championship]
                            INNER JOIN [Championship_Organization] [Championship_Organization] ON [Championship_Organization].IdChampionship = [Championship].Id
                            INNER JOIN [Organization] [Organization] ON [Organization].Id = [Championship_Organization].IdOrganization
                            INNER JOIN [Team] [Team] ON [Team].IdChampionship = [Championship].Id
                            INNER JOIN [RaceClass] [RaceClass] ON [RaceClass].Id = [Team].IdRaceClass
                            INNER JOIN [Team_Member] [Member] ON [Member].IdTeam = [Team].Id
                            INNER JOIN [User] [User] ON [User].Id = [Member].IdUser AND[User].IsActive = 1
                            INNER JOIN [User_Person] [User_Person] ON [User_Person].IdUser = [User].Id
                            INNER JOIN [Person] [Person] ON [Person].Id = [User_Person].IdPerson";

            QueryBuilder.AddCommand(sql);
            this.ProcessMemberSearchFilter(memberSearchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var members = new List<Member>();

            PaginatedResult<Member> items = base.GetPaginatedResults<Member>
                (
                    (reader) =>
                    {
                        return reader.Read<Member, Championship, Organization, Team, RaceClass, User, Person, Member>
                        (
                            (member, championship, organization, team, raceClass, user, person) =>
                            {
                                member.Championship = championship;
                                member.Championship.Organizations.Add(organization);
                                member.Team = team;
                                member.Team.RaceClass = raceClass;
                                member.User = user;
                                member.Person = person;

                                members.Add(member);

                                return member;
                            },
                            splitOn: "Id, Id, Id, Id, Id, Id"
                        );
                    },
                    context
                );

            //base.__FixPaginationResults(ref items, championshipMembers, paginationFilter);

            return items;
        }

        private void ProcessMemberSearchFilter(MemberSearchFilter memberSearchFilter)
        {
            if (memberSearchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.Equal, "Championship", "Id", "idChampionship", memberSearchFilter.Championship.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Organization", "Id", "idOrganization", memberSearchFilter.Organization?.Id);
            base.AddFilterCriteria(ConditionType.Equal, "Team", "Id", "idTeam", memberSearchFilter.Team?.Id);
            base.AddFilterCriteria(ConditionType.In, "RaceClass", "Id", "idRaceClass", memberSearchFilter.RaceClasses?.Select(x => x.Id));
        }

        #endregion
    }
}
