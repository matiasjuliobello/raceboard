using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class RaceProtestValidator : AbstractCustomValidator<RaceProtest>
    {
        private readonly IRaceProtestRepository _raceProtestRepository;

        public RaceProtestValidator
            (
                ITranslator translator,
                IRaceProtestRepository raceProtestRepository
            )
            : base(translator)
        {
            _raceProtestRepository = raceProtestRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            //RuleFor(x => x.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdIsRequired"))
            //    .When(x => Scenario == Scenario.Update);

            //RuleFor(x => x.RaceProtest.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdRaceProtestIsRequired"))
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


