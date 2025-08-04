using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class BoatOwnerValidator : AbstractCustomValidator<BoatOwner>
    {
        private readonly IBoatOwnerRepository _boatOwnerRepository;

        public BoatOwnerValidator
            (
                ITranslator translator,
                IBoatOwnerRepository boatOwnerRepository
            )
            : base(translator)
        {
            _boatOwnerRepository = boatOwnerRepository;

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
                .Must(x => !_boatOwnerRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}


