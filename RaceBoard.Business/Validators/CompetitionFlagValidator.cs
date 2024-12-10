using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Validators
{
    public class CompetitionFlagValidator : AbstractCustomValidator<CompetitionFlag>
    {
        private readonly ICompetitionFlagRepository _mastFlagRepository;
        private readonly ICompetitionRepository _mastRepository;
        private readonly IFlagRepository _flagRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IDateTimeHelper _dateTimeHelper;

        public CompetitionFlagValidator
            (
                ITranslator translator,
                ICompetitionFlagRepository mastFlagRepository,
                ICompetitionRepository mastRepository,
                IFlagRepository flagRepository,
                IPersonRepository personRepository,
                IDateTimeHelper dateTimeHelper
            )
            : base(translator)
        {
            _mastFlagRepository = mastFlagRepository;
            _mastRepository = mastRepository;
            _flagRepository = flagRepository;
            _personRepository = personRepository;
            _dateTimeHelper = dateTimeHelper;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            //RuleFor(x => x.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdIsRequired"))
            //    .When(x => Scenario == Scenario.Update || Scenario == Scenario.Delete);

            //RuleFor(x => x.Competition.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdCompetitionIsRequired"))
            //    .When(x => Scenario == Scenario.Create);

            //RuleFor(x => x.Competition.Id)
            //    .Must(x => _mastRepository.Exists(x))
            //    .WithMessage(Translate("IdCompetitionIsNotValid"))
            //    .When(x => Scenario == Scenario.Create);

            //RuleFor(x => x.Flag.Id)
            //    .NotEmpty()
            //    .WithMessage(Translate("IdFlagIsRequired"))
            //    .When(x => Scenario == Scenario.Create);

            //RuleFor(x => x.Flag.Id)
            //    .Must(x => _flagRepository.Exists(x))
            //    .WithMessage(Translate("IdFlagIsNotValid"))
            //    .When(x => Scenario == Scenario.Create);

            ////RuleFor(x => x.Person.Id)
            ////    .NotEmpty()
            ////    .WithMessage(Translate("IdPersonIsRequired"))
            ////    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            ////RuleFor(x => x.Person.Id)
            ////    .Must(x => _personRepository.Exists(x, base.TransactionalContext))
            ////    .WithMessage(Translate("IdPersonIsNotValid"))
            ////    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x)
            //    .Must(x => !_mastFlagRepository.ExistsDuplicate(x, base.TransactionalContext))
            //    .WithMessage(Translate("DuplicateRecordExists"))
            //    .When(x => Scenario == Scenario.Create);

            //RuleFor(x => x.Raising)
            //    .Must(x => x < _dateTimeHelper.GetCurrentTimestamp())
            //    .WithMessage(Translate("LoweringCannotBeInThePast"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x.Lowering)
            //    .Must(x => x < _dateTimeHelper.GetCurrentTimestamp())
            //    .WithMessage(Translate("LoweringCannotBeInThePast"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x)
            //    .Must(x => x.Lowering > x.Raising)
            //    .WithMessage(Translate("LoweringMustOccurAfterRaising"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}