using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Validators
{
    public class TeamMemberInvitationValidator : AbstractCustomValidator<TeamMemberInvitation>
    {
        private readonly ITeamMemberRepository _teamMemberRepository;

        public TeamMemberInvitationValidator
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
            //RuleFor(x => x.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdIsRequired"))
            //    .When(x => Scenario == Scenario.Update);

            RuleFor(x => x.Role.Id)
                .NotEmpty()
                .WithMessage(Translate("IdRoleIsRequired"))
                .When(x => Scenario == Scenario.Create);

            //RuleFor(x => x)
            //    .Must(x => !_teamMemberRepository.ExistsDuplicate(x, base.TransactionalContext))
            //    .WithMessage(Translate("DuplicateRecordExists"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => (x.User != null || !string.IsNullOrEmpty(x.Invitation.EmailAddress)))
                .WithMessage(Translate("PersonOrEmailAddressMustBeProvided"))
                .When(x => Scenario == Scenario.Create);

            //RuleFor(x => x)
            //    .Must(x => !_teamMemberRepository.HasParticipationOnRace(x, base.TransactionalContext))
            //    .WithMessage(Translate("CannotDeleteTeamMemberDueToExistingParticipation"))
            //    .When(x => Scenario == Scenario.Delete);
        }
    }
}


