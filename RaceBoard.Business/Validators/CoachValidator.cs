using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class CoachValidator : AbstractCustomValidator<Coach>
    {
        private readonly ICoachRepository _coachRepository;

        public CoachValidator
            (
                ITranslator translator,
                ICoachRepository coachRepository
            )
            : base(translator)
        {
            _coachRepository = coachRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_coachRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}