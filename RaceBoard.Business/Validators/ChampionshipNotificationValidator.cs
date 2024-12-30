using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class ChampionshipNotificationValidator : AbstractCustomValidator<ChampionshipNotification>
    {
        private readonly IChampionshipNotificationRepository _championshipNotificationRepository;

        public ChampionshipNotificationValidator
            (
                ITranslator translator,
                IChampionshipNotificationRepository championshipNotificationRepository
            )
            : base(translator)
        {
            _championshipNotificationRepository = championshipNotificationRepository;

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


