using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Common.Helpers.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class MastFlagValidator : AbstractCustomValidator<MastFlag>
    {
        private readonly IMastFlagRepository _mastFlagRepository;
        private readonly IMastRepository _mastRepository;
        private readonly IFlagRepository _flagRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IDateTimeHelper _dateTimeHelper;

        public MastFlagValidator
            (
                ITranslator translator,
                IMastFlagRepository mastFlagRepository,
                IMastRepository mastRepository,
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
            RuleFor(x => x.Mast.Id)
                .NotEmpty()
                .WithMessage(Translate("IdMastIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Mast.Id)
                .Must(x => _mastRepository.Exists(x))
                .WithMessage(Translate("IdMastIsNotValid"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Flag.Id)
                .NotEmpty()
                .WithMessage(Translate("IdFlagIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Flag.Id)
                .Must(x => _flagRepository.Exists(x))
                .WithMessage(Translate("IdFlagIsNotValid"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Person.Id)
                .NotEmpty()
                .WithMessage(Translate("IdPersonIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Person.Id)
                .Must(x => _personRepository.Exists(x, base.TransactionalContext))
                .WithMessage(Translate("IdPersonIsNotValid"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => !_mastFlagRepository.ExistsDuplicate(x, base.TransactionalContext))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create);

            //RuleFor(x => x.RaisingMoment)
            //    .Must(x => x < _dateTimeHelper.GetCurrentTimestamp())
            //    .WithMessage(Translate("LoweringMomentCannotBeInThePast"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            //RuleFor(x => x.LoweringMoment)
            //    .Must(x => x < _dateTimeHelper.GetCurrentTimestamp())
            //    .WithMessage(Translate("LoweringMomentCannotBeInThePast"))
            //    .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x)
                .Must(x => x.LoweringMoment > x.RaisingMoment)
                .WithMessage(Translate("LoweringMomentMustOccurAfterRaisingMoment"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}