using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Pagination;

namespace RaceBoard.Business.Managers
{
    public class TeamMemberRoleManager : AbstractManager, ITeamMemberRoleManager
    {
        private readonly ITeamMemberRoleRepository _teamMemberRoleRepository;

        #region Constructors

        public TeamMemberRoleManager
            (
                ITeamMemberRoleRepository teamMemberRoleRepository,
                ITranslator translator
            ) : base(translator)
        {
            _teamMemberRoleRepository = teamMemberRoleRepository;
        }

        #endregion

        #region ITeamMemberRoleManager implementation

        public PaginatedResult<TeamMemberRole> Get(TeamMemberRoleSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _teamMemberRoleRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public TeamMemberRole Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new TeamMemberRoleSearchFilter() { Ids = new int[] { id } };

            var teamMemberRoles = _teamMemberRoleRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var teamMemberRole = teamMemberRoles.Results.FirstOrDefault();
            if (teamMemberRole == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return teamMemberRole;
        }

        #endregion
    }
}