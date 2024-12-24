using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Validators
{
    public class TeamMemberValidator : AbstractCustomValidator<TeamMember>
    {
        private readonly ITeamMemberRepository _teamMemberRepository;

        public TeamMemberValidator
            (
                ITranslator translator,
                ITeamMemberRepository teamMemberRepository
            )
            : base(translator)
        {
            _teamMemberRepository = teamMemberRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x.Role.Id)
                .NotEmpty()
                .WithMessage(Translate("IdRoleIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_teamMemberRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            // Esta validación estaría bien si cada Rol fuese único, pero "Tripulante" puede haber varios..
            //RuleFor(x => x)
            //    .Must(x => !_teamMemberRepository.HasDuplicatedRole(x, base.TransactionalContext))
            //    .WithMessage(Translate("RoleIsAlreadyAssignedToAnotherMember"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_teamMemberRepository.HasParticipationOnRace(x, base.TransactionalContext))
                .WithMessage(Translate("CannotDeleteTeamMemberDueToExistingParticipation"))
                .When(x => Scenario == Scenario.Delete);


        }
    }
}


