using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Data.Repositories;

namespace RaceBoard.Business.Validators
{
    public class RaceCommitteeBoatReturnValidator : AbstractCustomValidator<RaceCommitteeBoatReturn>
    {
        private readonly IRaceCommitteeBoatReturnRepository _raceCommitteeBoatReturnRepository;

        public RaceCommitteeBoatReturnValidator
            (
                ITranslator translator,
                IRaceCommitteeBoatReturnRepository raceCommitteeBoatReturnRepository
            )
            : base(translator)
        {
            _raceCommitteeBoatReturnRepository = raceCommitteeBoatReturnRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            //RuleFor(x => x.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdIsRequired"))
            //    .When(x => Scenario == Scenario.Update);

            //RuleFor(x => x.RaceCommitteeBoatReturn.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdRaceCommitteeBoatReturnIsRequired"))
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


