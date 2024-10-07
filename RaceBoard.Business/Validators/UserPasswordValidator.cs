using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using System.Text.RegularExpressions;

namespace RaceBoard.Business.Validators
{
    public class UserPasswordValidator : AbstractCustomValidator<UserPassword>
    {
        private PasswordPolicy _passwordPolicy;

        protected static class RegularExpressions
        {
            public static Regex Numbers = new Regex("[\\d]");
            public static Regex LowercaseChars = new Regex("[a-z]");
            public static Regex UppercaseChars = new Regex("[A-Z]");
            public static Regex SpecialChars = new Regex("[!?|@#$&%=(){}.+,_-]"); // Allowed Chars: ! ? | @ # $ & % = ( ) { } . + , _ -
        }

        public UserPasswordValidator(ITranslator translator)
            : base(translator)
        {
            base.SetRules(this.AddRules);
        }

        #region Private Methods

        private void AddRules()
        {
            #region Context Setting

            if (base.Context == null)
            {
                var passwordPolicy = new PasswordPolicy()
                {
                    MinLength = 8,
                    MaxLength = 20,
                    MinLowercaseChars = 1,
                    MinUppercaseChars = 1,
                    MinNumericChars = 1,
                    MinSpecialChars = 1
                };
                base.SetContext(passwordPolicy);
            }

            _passwordPolicy = base.Context as PasswordPolicy;

            if (_passwordPolicy == null)
                throw new FunctionalException(ErrorType.NotFound, Translate("PasswordPolicyNotFound"));

            #endregion

            #region Rules

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(Translate("PasswordIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Password)
                .Must(x => x.Length >= _passwordPolicy.MinLength)
                .WithMessage(Translate("PasswordMustHaveAtLeast_N_Characters", _passwordPolicy.MinLength))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Password)
                .Must(x => x.Length <= _passwordPolicy.MaxLength)
                .WithMessage(Translate("PasswordCannotHaveMoreThan_N_Characters", _passwordPolicy.MaxLength))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Password)
                .Must(x => RegularExpressions.Numbers.Matches(x).Count >= _passwordPolicy.MinNumericChars)
                .WithMessage(Translate("PasswordMustHaveAtLeast_N_NumericCharacters", _passwordPolicy.MinNumericChars))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Password)
                .Must(x => RegularExpressions.SpecialChars.Matches(x).Count >= _passwordPolicy.MinSpecialChars)
                .WithMessage(Translate("PasswordMustHaveAtLeast_N_SpecialCharacters", _passwordPolicy.MinSpecialChars))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Password)
                .Must(x => RegularExpressions.LowercaseChars.Matches(x).Count >= _passwordPolicy.MinLowercaseChars)
                .WithMessage(Translate("PasswordMustHaveAtLeast_N_LowercaseCharacters", _passwordPolicy.MinLowercaseChars))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Password)
                .Must(x => RegularExpressions.UppercaseChars.Matches(x).Count >= _passwordPolicy.MinUppercaseChars)
                .WithMessage(Translate("PasswordMustHaveAtLeast_N_UppercaseCharacters", _passwordPolicy.MinUppercaseChars))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            #endregion
        }

        #endregion
    }
}
