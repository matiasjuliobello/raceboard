using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class CrewChangeRequestValidator : AbstractCustomValidator<CrewChangeRequest>
    {
        private readonly ICrewChangeRequestRepository _crewChangeRequestRepository;

        public CrewChangeRequestValidator
            (
                ITranslator translator,
                ICrewChangeRequestRepository crewChangeRequestRepository
            )
            : base(translator)
        {
            _crewChangeRequestRepository = crewChangeRequestRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
        }
    }
}
