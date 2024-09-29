﻿using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class TeamBoatValidator : AbstractCustomValidator<TeamBoat>
    {
        private readonly ITeamBoatRepository _teamBoatRepository;

        public TeamBoatValidator
            (
                ITranslator translator,
                ITeamBoatRepository teamBoatRepository
            )
            : base(translator)
        {
            _teamBoatRepository = teamBoatRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x.Team.Id)
                .NotEmpty()
                .WithMessage(Translate("IdTeamIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Boat.Id)
                .NotEmpty()
                .WithMessage(Translate("IdBoatIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_teamBoatRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("BoatAlreadyAssignedToAnotherTeam"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}


