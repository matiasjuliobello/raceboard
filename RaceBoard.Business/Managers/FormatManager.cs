using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

public class FormatManager : AbstractManager, IFormatManager
{
    private readonly IFormatRepository _formatRepositoryRepository;

    #region Constructors

    public FormatManager
        (
            IFormatRepository formatRepositoryRepository,
            ITranslator translator
        ) : base(translator)
    {
        _formatRepositoryRepository = formatRepositoryRepository;
    }

    #endregion

    #region IFormatManager implementation

    public List<DateFormat> GetDateFormats(ITransactionalContext? context = null)
    {
        return _formatRepositoryRepository.GetDateFormats(context);
    }

    #endregion

    #region Private Methods

    #endregion
}