using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class CompetitionNewsUpdateValidator : AbstractCustomValidator<CompetitionNewsUpdate>
    {
        private readonly ICompetitionNewsUpdateRepository _competitionNewsUpdateRepository;

        public CompetitionNewsUpdateValidator
            (
                ITranslator translator,
                ICompetitionNewsUpdateRepository competitionNewsUpdateRepository
            )
            : base(translator)
        {
            _competitionNewsUpdateRepository = competitionNewsUpdateRepository;

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


