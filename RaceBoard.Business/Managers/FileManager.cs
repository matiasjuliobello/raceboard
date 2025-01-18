using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class FileManager : AbstractManager, IFileManager
    {
        private readonly IFileRepository _fileRepository;

        #region Constructors

        public FileManager
            (
                IFileRepository fileRepository,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _fileRepository = fileRepository;
        }

        #endregion

        #region IFileManager implementation

        public Domain.File Get(int id, ITransactionalContext? context = null)
        {
            var file = _fileRepository.Get(id, context );
            if (file == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return file;
        }

        public Domain.File Get(string url, ITransactionalContext? context = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}