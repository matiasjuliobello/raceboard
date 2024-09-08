﻿using Dapper;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using System;

namespace RaceBoard.Data.Repositories
{
    public class MastRepository : AbstractRepository, IMastRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _mastColumnsMapping = new()
        {
            { "Id", "[Mast].Id" },
            { "Competition.Id", "[Competition].Id" },
            { "Competition.Name", "[Competition].Name" }
        };

        #endregion

        #region Constructors

        public MastRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IMastRepository implementation

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

        public bool Exists(int id, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsQuery("[Mast]", "[Id] = @id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("id", id);

            return base.Execute<bool>(context);
        }

        public bool ExistsDuplicate(Mast mast, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsDuplicateQuery("[Mast]", "[IdCompetition] = @idCompetition", "Id", "@id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("id", mast.Id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Mast> Get(MastSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return this.GetMasts(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        public void Create(Mast mast, ITransactionalContext? context = null)
        {
            this.CreateMast(mast, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Mast> GetMasts(MastSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Mast].Id [Id],
                                [Competition].Id [Id],
                                [Competition].Name [Name]
                            FROM [Mast] [Mast]
                            INNER JOIN [Competition] [Competition] ON [Competition].Id = [Mast].IdCompetition";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _mastColumnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            var masts = new List<Mast>();

            PaginatedResult<Mast> items = base.GetPaginatedResults<Mast>
                (
                    (reader) =>
                    {
                        return reader.Read<Mast, Competition, Mast>
                        (
                            (mast, competition) =>
                            {
                                mast.Competition = competition;

                                masts.Add(mast);

                                return mast;
                            },
                            splitOn: "Id, Id"
                        ).AsList();
                    },
                    context
                );

            items.Results = masts;

            return items;
        }

        private void ProcessSearchFilter(MastSearchFilter searchFilter)
        {
            if (searchFilter.Ids != null && searchFilter.Ids.Length > 0)
            {
                QueryBuilder.AddCondition($"[Mast].Id IN @ids");
                QueryBuilder.AddParameter("ids", searchFilter.Ids);
            }

            if (searchFilter.IdCompetition.HasValue && searchFilter.IdCompetition > 0 )
            {
                QueryBuilder.AddCondition($"[Mast].IdCompetition = @idCompetition");
                QueryBuilder.AddParameter("idCompetition", searchFilter.IdCompetition.Value);
            }
        }

        private void CreateMast(Mast mast, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [Mast]
                                ( IdCompetition )
                            VALUES
                                ( @idCompetition )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idCompetition", mast.Competition.Id);

            QueryBuilder.AddReturnLastInsertedId();

            mast.Id = base.Execute<int>(context);
        }

        #endregion
    }
}