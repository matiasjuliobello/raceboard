using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class MastValidator : AbstractCustomValidator<Mast>
    {
        private readonly IMastRepository _mastRepository;

        public MastValidator
            (
                ITranslator translator,
                IMastRepository boatRepository
            )
            : base(translator)
        {
            _mastRepository = boatRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x.Competition.Id)
                .NotEmpty()
                .WithMessage(Translate("IdCompetitionIsRequired"))
                .When(x => Scenario == Scenario.Create);

            RuleFor(x => x)
                .Must(x => !_mastRepository.ExistsDuplicate(x))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create);
        }
    }
}