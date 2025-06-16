using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers
{
    public class MemberManager : AbstractManager, IMemberManager
    {
        private readonly IMemberRepository _memberRepository;

        #region Constructors

        public MemberManager
            (
                IMemberRepository memberRepository,
                ITranslator translator,
                IRequestContextManager requestContextManager
            ) : base(requestContextManager, translator)
        {
            _memberRepository = memberRepository;
        }

        #endregion

        #region IMemberManager implementation

        public PaginatedResult<Member> Get(MemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _memberRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        #endregion
    }
}
