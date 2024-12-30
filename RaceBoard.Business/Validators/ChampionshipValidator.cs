using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class ChampionshipValidator : AbstractCustomValidator<Championship>
    {
        private readonly IChampionshipRepository _championshipRepository;

        public ChampionshipValidator
            (
                ITranslator translator,
                IChampionshipRepository championshipRepository
            )
            : base(translator)
        {
            _championshipRepository = championshipRepository;

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
                .WithMessage(Translate("Name"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.City.Id)
                .NotEmpty()
                .WithMessage(Translate("CityIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Organizations)
                .NotEmpty()
                .WithMessage(Translate("OrganizationsIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x.StartDate)
            //    .NotEmpty()
            //    .WithMessage(Translate("StartDateIsRequired"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x.EndDate)
            //    .NotEmpty()
            //    .WithMessage(Translate("EndDateIsRequired"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_championshipRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}


