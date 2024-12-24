using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IOrganizationMemberRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(OrganizationMember organizationMember, ITransactionalContext? context = null);
        PaginatedResult<OrganizationMember> Get(OrganizationMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        PaginatedResult<OrganizationMemberInvitation> GetInvitations(OrganizationMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Add(OrganizationMember organizationMember, ITransactionalContext? context = null);
        int Remove(int id, ITransactionalContext? context = null);
        void CreateInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null);
        void UpdateInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null);
        void RemoveInvitation(OrganizationMemberInvitation organizationMemberInvitation, ITransactionalContext? context = null);
    }
}