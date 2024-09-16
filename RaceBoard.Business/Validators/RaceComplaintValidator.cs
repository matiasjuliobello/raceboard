using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class RaceComplaintValidator : AbstractCustomValidator<RaceComplaint>
    {
        private readonly IRaceRepository _raceRepository;

        public RaceComplaintValidator
            (
                ITranslator translator,
                IRaceRepository raceRepository
            )
            : base(translator)
        {
            _raceRepository = raceRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            //RuleFor(x => x.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdIsRequired"))
            //    .When(x => Scenario == Scenario.Update);

            //RuleFor(x => x.RaceComplaint.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdRaceComplaintIsRequired"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x.Boat.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdBoatIsRequired"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x)
            //    .Must(x => !_teamBoatRepository.ExistsDuplicate(x, base.TransactionalContext))
            //    .WithMessage(Translate("DuplicateRecordExists"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}


