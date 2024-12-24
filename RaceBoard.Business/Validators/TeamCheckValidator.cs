using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class TeamCheckValidator : AbstractCustomValidator<TeamMemberCheck>
    {
        private readonly ITeamMemberCheckRepository _teamCheckRepository;

        public TeamCheckValidator
            (
                ITranslator translator,
                ITeamMemberCheckRepository teamCheckRepository
            )
            : base(translator)
        {
            _teamCheckRepository = teamCheckRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            //RuleFor(x => x.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdIsRequired"))
            //    .When(x => Scenario == Scenario.Update);

            //RuleFor(x => x.Team.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdTeamIsRequired"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x.Boat.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdBoatIsRequired"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x)
            //    .Must(x => !_teamCheckRepository.ExistsDuplicate(x, base.TransactionalContext))
            //    .WithMessage(Translate("BoatAlreadyAssignedToAnotherTeam"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}


