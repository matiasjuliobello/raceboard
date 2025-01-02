using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class FileTypeManager : AbstractManager, IFileTypeManager
    {
        private readonly IFileTypeRepository _fileTypeRepository;

        #region Constructors

        public FileTypeManager
            (
                IFileTypeRepository fileTypeRepository,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _fileTypeRepository = fileTypeRepository;
        }

        #endregion

        #region IFileTypeManager implementation

        public PaginatedResult<FileType> Get(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _fileTypeRepository.Get(paginationFilter, sorting, context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
