using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class CompetitionMemberValidator : AbstractCustomValidator<CompetitionMember>
    {
        private readonly ICompetitionMemberRepository _competitionMemberRepository;

        public CompetitionMemberValidator
            (
                ITranslator translator,
                ICompetitionMemberRepository competitionMemberRepository
            )
            : base(translator)
        {
            _competitionMemberRepository = competitionMemberRepository;

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

            //RuleFor(x => x)
            //    .Must(x => !_competitionMemberRepository.ExistsDuplicate(x, base.TransactionalContext))
            //    .WithMessage(Translate("DuplicateRecordExists"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x)
            //    .Must(x => (x.User != null || !string.IsNullOrEmpty(x.Invitation.EmailAddress)))
            //    .WithMessage(Translate("PersonOrEmailAddressMustBeProvided"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x)
            //    .Must(x => !_competitionMemberRepository.HasParticipationOnRace(x, base.TransactionalContext))
            //    .WithMessage(Translate("CannotDeleteCompetitionMemberDueToExistingParticipation"))
            //    .When(x => Scenario == Scenario.Delete);
        }
    }
}


