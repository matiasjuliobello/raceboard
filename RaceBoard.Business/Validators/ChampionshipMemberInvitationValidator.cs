using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Validators
{
    public class ChampionshipMemberInvitationValidator : AbstractCustomValidator<ChampionshipMemberInvitation>
    {
        private readonly IChampionshipMemberRepository _championshipMemberRepository;

        public ChampionshipMemberInvitationValidator
            (
                ITranslator translator,
                IChampionshipMemberRepository championshipMemberRepository
            )
            : base(translator)
        {
            _championshipMemberRepository = championshipMemberRepository;

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
            //    .Must(x => !_championshipMemberRepository.ExistsDuplicate(x, base.TransactionalContext))
            //    .WithMessage(Translate("DuplicateRecordExists"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => (x.User != null || !string.IsNullOrEmpty(x.Invitation.EmailAddress)))
                .WithMessage(Translate("PersonOrEmailAddressMustBeProvided"))
                .When(x => Scenario == Scenario.Create);

            //RuleFor(x => x)
            //    .Must(x => !_championshipMemberRepository.HasParticipationOnRace(x, base.TransactionalContext))
            //    .WithMessage(Translate("CannotDeleteChampionshipMemberDueToExistingParticipation"))
            //    .When(x => Scenario == Scenario.Delete);
        }
    }
}


