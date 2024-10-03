using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data.Repositories;

namespace RaceBoard.Business.Validators
{
    public class CompetitionGroupValidator : AbstractCustomValidator<CompetitionGroup>
    {
        private readonly ICompetitionGroupRepository _competitionGroupRepository;

        public CompetitionGroupValidator
            (
                ITranslator translator,
                ICompetitionGroupRepository competitionGroupRepository
            )
            : base(translator)
        {
            _competitionGroupRepository = competitionGroupRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x)
                .Must(x => this.CheckDateRangeIsValid(x.RegistrationStartDate, x.RegistrationEndDate))
                .WithMessage(Translate("RegistrationDatesAreInvalid"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
            
            RuleFor(x => x)
                .Must(x => this.CheckDateRangeIsValid(x.AccreditationStartDate, x.AccreditationEndDate))
                .WithMessage(Translate("AccreditationDatesAreInvalid"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
            
            RuleFor(x => x)
                .Must(x => this.CheckDateRangeIsValid(x.CompetitionStartDate, x.CompetitionEndDate))
                .WithMessage(Translate("CompetitionDatesAreInvalid"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => x.AccreditationStartDate >= x.RegistrationEndDate)
                .WithMessage(Translate("AccreditationCannotOccureBeforeRegistration"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => x.CompetitionStartDate >= x.RegistrationEndDate)
                .WithMessage(Translate("CompetitionCannotOccureBeforeRegistration"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => x.CompetitionStartDate >= x.AccreditationEndDate)
                .WithMessage(Translate("CompetitionCannotOccureBeforeAccreditation"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(Translate("NameIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.RaceClasses)
                .Must(x => x.Count > 0)
                .WithMessage(Translate("AtLeastOneRaceClassMustBeSelected"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_competitionGroupRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

        }

        private bool CheckDateRangeIsValid(DateTimeOffset start, DateTimeOffset end)
        {
            return start <= end;
        }
    }
}


