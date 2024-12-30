using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class ChampionshipFileValidator : AbstractCustomValidator<ChampionshipFile>
    {
        private readonly IChampionshipFileRepository _championshipFileRepository;

        public ChampionshipFileValidator
            (
                ITranslator translator,
                IChampionshipFileRepository championshipFileRepository
            )
            : base(translator)
        {
            _championshipFileRepository = championshipFileRepository;

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


