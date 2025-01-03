﻿using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data.Repositories;

namespace RaceBoard.Business.Validators
{
    public class ChampionshipGroupValidator : AbstractCustomValidator<ChampionshipGroup>
    {
        private readonly IChampionshipGroupRepository _championshipGroupRepository;

        public ChampionshipGroupValidator
            (
                ITranslator translator,
                IChampionshipGroupRepository championshipGroupRepository
            )
            : base(translator)
        {
            _championshipGroupRepository = championshipGroupRepository;

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
                .Must(x => this.CheckDateRangeIsValid(x.ChampionshipStartDate, x.ChampionshipEndDate))
                .WithMessage(Translate("ChampionshipDatesAreInvalid"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => x.AccreditationStartDate >= x.RegistrationEndDate)
                .WithMessage(Translate("AccreditationCannotOccureBeforeRegistration"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => x.ChampionshipStartDate >= x.RegistrationEndDate)
                .WithMessage(Translate("ChampionshipCannotOccureBeforeRegistration"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => x.ChampionshipStartDate >= x.AccreditationEndDate)
                .WithMessage(Translate("ChampionshipCannotOccureBeforeAccreditation"))
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
                .Must(x => !_championshipGroupRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

        }

        private bool CheckDateRangeIsValid(DateTimeOffset start, DateTimeOffset end)
        {
            return start <= end;
        }
    }
}


