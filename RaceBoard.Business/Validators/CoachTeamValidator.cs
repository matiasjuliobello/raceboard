using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class CoachTeamValidator : AbstractCustomValidator<CoachTeam>
    {
        private readonly ICoachTeamRepository _coachTeamRepository;

        public CoachTeamValidator
            (
                ITranslator translator,
                ICoachTeamRepository coachTeamRepository
            )
            : base(translator)
        {
            _coachTeamRepository = coachTeamRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_coachTeamRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}
