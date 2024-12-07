using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class CompetitionFileValidator : AbstractCustomValidator<CompetitionFile>
    {
        private readonly ICompetitionFileRepository _competitionFileRepository;

        public CompetitionFileValidator
            (
                ITranslator translator,
                ICompetitionFileRepository competitionFileRepository
            )
            : base(translator)
        {
            _competitionFileRepository = competitionFileRepository;

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


