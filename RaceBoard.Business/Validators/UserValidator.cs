using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Data.Repositories.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class UserValidator : AbstractCustomValidator<User>
    {
        private readonly IUserRepository _userRepository;

        public UserValidator
            (
                ITranslator translator,
                IUserRepository userRepository
            )
            : base(translator)
        {
            _userRepository = userRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage(Translate("UsernameIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            // TODO: validate email with Regular Expression
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(Translate("EmailIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Firstname)
                .NotEmpty()
                .WithMessage(Translate("FirstnameIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Lastname)
                .NotEmpty()
                .WithMessage(Translate("LastnameIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithMessage(Translate("BirthDateIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.IsActive)
                .Must(x => x)
                .WithMessage(Translate("IsActiveCannotBeFalseOnCreation"))
                .When(x => Scenario == Scenario.Create);

            RuleFor(x => x)
                .Must(x => !_userRepository.ExistsDuplicate(x))
                .WithMessage(Translate("DuplicateRecordExists"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}
