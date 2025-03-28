using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class HearingRequestValidator : AbstractCustomValidator<HearingRequest>
    {
        private readonly IHearingRequestRepository _hearingRequestRepository;

        public HearingRequestValidator
            (
                ITranslator translator,
                IHearingRequestRepository hearingRequestRepository
            )
            : base(translator)
        {
            _hearingRequestRepository = hearingRequestRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
        }
    }
}
