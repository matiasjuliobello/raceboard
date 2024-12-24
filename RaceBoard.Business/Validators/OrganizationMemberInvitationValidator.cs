using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Validators
{
    public class OrganizationMemberInvitationValidator : AbstractCustomValidator<OrganizationMemberInvitation>
    {
        private readonly IOrganizationMemberRepository _organizationMemberRepository;

        public OrganizationMemberInvitationValidator
            (
                ITranslator translator,
                IOrganizationMemberRepository organizationMemberRepository
            )
            : base(translator)
        {
            _organizationMemberRepository = organizationMemberRepository;

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
            //    .Must(x => !_organizationMemberRepository.ExistsDuplicate(x, base.TransactionalContext))
            //    .WithMessage(Translate("DuplicateRecordExists"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => (x.User != null || !string.IsNullOrEmpty(x.Invitation.EmailAddress)))
                .WithMessage(Translate("PersonOrEmailAddressMustBeProvided"))
                .When(x => Scenario == Scenario.Create);

            //RuleFor(x => x)
            //    .Must(x => !_organizationMemberRepository.HasParticipationOnRace(x, base.TransactionalContext))
            //    .WithMessage(Translate("CannotDeleteOrganizationMemberDueToExistingParticipation"))
            //    .When(x => Scenario == Scenario.Delete);
        }
    }
}


