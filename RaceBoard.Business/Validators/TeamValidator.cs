using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class TeamValidator : AbstractCustomValidator<Team>
    {
        private readonly ITeamRepository _teamRepository;

        public TeamValidator
            (
                ITranslator translator,
                ITeamRepository teamRepository
            )
            : base(translator)
        {
            _teamRepository = teamRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            //RuleFor(x => x.Name)
            //    .Must(x => !string.IsNullOrEmpty(x))
            //    .WithMessage(Translate("NameIsRequired"))
            //    .Must(x => x.Length > 3)
            //    .WithMessage(Translate("NameIsTooShort"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
                /*.When(x => !string.IsNullOrEmpty(x.Name))*/;

            //RuleFor(x => x.Organization.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdOrganizationIsRequired"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Competition.Id)
                .NotEmpty()
                .WithMessage(Translate("IdCompetitionIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);


            RuleFor(x => x.RaceClass.Id)
                .NotEmpty()
                .WithMessage(Translate("IdRaceClassIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x.Boat.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdBoatIsRequired"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_teamRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}


