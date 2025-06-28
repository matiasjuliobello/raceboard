using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IOrganizationMemberManager
    {
        PaginatedResult<OrganizationMember> Get(OrganizationMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);        
        OrganizationMember Get(int id, ITransactionalContext? context = null);
        PaginatedResult<OrganizationMemberInvitation> GetMemberInvitations(int idOrganization, bool isPending, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        OrganizationMemberInvitation GetInvitation(int id, ITransactionalContext? context = null);
        void CreateInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null);
        void UpdateInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null);
        void Remove(int id, ITransactionalContext? context = null);
        void RemoveInvitation(int id, ITransactionalContext? context = null);
    }
}