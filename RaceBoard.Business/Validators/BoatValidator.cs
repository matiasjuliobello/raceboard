using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class BoatValidator : AbstractCustomValidator<Boat>
    {
        private readonly IBoatRepository _boatRepository;

        public BoatValidator
            (
                ITranslator translator,
                IBoatRepository boatRepository
            )
            : base(translator)
        {
            _boatRepository = boatRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(Translate("NameIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.SailNumber)
                .NotEmpty()
                .WithMessage(Translate("SailNumberIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_boatRepository.ExistsDuplicate(x))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}


