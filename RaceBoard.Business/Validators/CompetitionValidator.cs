using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class CompetitionValidator : AbstractCustomValidator<Competition>
    {
        private readonly ICompetitionRepository _competitionRepository;

        public CompetitionValidator
            (
                ITranslator translator,
                ICompetitionRepository competitionRepository
            )
            : base(translator)
        {
            _competitionRepository = competitionRepository;

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

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage(Translate("StartDateIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage(Translate("EndDateIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_competitionRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}


