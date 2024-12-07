using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class CompetitionNotificationValidator : AbstractCustomValidator<CompetitionNotification>
    {
        private readonly ICompetitionNotificationRepository _competitionNotificationRepository;

        public CompetitionNotificationValidator
            (
                ITranslator translator,
                ICompetitionNotificationRepository competitionNotificationRepository
            )
            : base(translator)
        {
            _competitionNotificationRepository = competitionNotificationRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            //RuleFor(x => x.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdIsRequired"))
            //    .When(x => Scenario == Scenario.Update);
        }
    }
}


