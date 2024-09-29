using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class CompetitionGroupValidator : AbstractCustomValidator<CompetitionGroup>
    {
        private readonly ICompetitionGroupRepository _competitionGroupRepository;

        public CompetitionGroupValidator
            (
                ITranslator translator,
                ICompetitionGroupRepository competitionGroupRepository
            )
            : base(translator)
        {
            _competitionGroupRepository = competitionGroupRepository;

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


