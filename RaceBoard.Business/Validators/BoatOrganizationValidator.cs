using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class BoatOrganizationValidator : AbstractCustomValidator<BoatOrganization>
    {
        private readonly IBoatOrganizationRepository _boatOrganizationRepository;

        public BoatOrganizationValidator
            (
                ITranslator translator,
                IBoatOrganizationRepository boatOrganizationRepository
            )
            : base(translator)
        {
            _boatOrganizationRepository = boatOrganizationRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x.EndDate)
                .Must(x => x <= DateTime.UtcNow)
                .WithMessage(Translate("EndDateCannotBeInTheFuture"))
                .When(x => x.EndDate != null && Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_boatOrganizationRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}


