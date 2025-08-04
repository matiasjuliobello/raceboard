using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class CoachOrganizationValidator : AbstractCustomValidator<CoachOrganization>
    {
        private readonly ICoachOrganizationRepository _coachOrganizationRepository;

        public CoachOrganizationValidator
            (
                ITranslator translator,
                ICoachOrganizationRepository coachOrganizationRepository
            )
            : base(translator)
        {
            _coachOrganizationRepository = coachOrganizationRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_coachOrganizationRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage(Translate("EndDateIsRequired"))
                .When(x => Scenario == Scenario.Update);
        }
    }
}
