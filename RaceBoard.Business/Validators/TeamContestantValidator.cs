using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class TeamContestantValidator : AbstractCustomValidator<TeamContestant>
    {
        private readonly ITeamContestantRepository _teamContestantRepository;

        public TeamContestantValidator
            (
                ITranslator translator,
                ITeamContestantRepository teamContestantRepository
            )
            : base(translator)
        {
            _teamContestantRepository = teamContestantRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_teamContestantRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}


