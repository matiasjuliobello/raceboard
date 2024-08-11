using RaceBoard.Common.Enums;
using RaceBoard.Data;

namespace RaceBoard.Business.Validators.Interfaces
{
    public interface ICustomValidator<T>
    {
        bool IsValid(T entity, Scenario scenario, bool? persist = false, string? identifier = null);

        List<string> Errors { get; }

        void SetTransactionalContext(ITransactionalContext transactionalContext);
        void SetContext(object context);
    }
}
