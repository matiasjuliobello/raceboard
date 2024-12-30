using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Data.Repositories;
using RaceBoard.Domain;

namespace RaceBoard.Business.Validators
{
    public class RaceCommitteeBoatReturnValidator : AbstractCustomValidator<ChampionshipBoatReturn>
    {
        private readonly IChampionshipCommitteeBoatReturnRepository _committeeBoatReturnRepository;

        public RaceCommitteeBoatReturnValidator
            (
                ITranslator translator,
                IChampionshipCommitteeBoatReturnRepository committeeBoatReturnRepository
            )
            : base(translator)
        {
            _committeeBoatReturnRepository = committeeBoatReturnRepository;

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


