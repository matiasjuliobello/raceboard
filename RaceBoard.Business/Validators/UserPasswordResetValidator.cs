using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Common.Enums;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Validators
{
    public class UserPasswordResetValidator : AbstractCustomValidator<UserPasswordReset>
    {
        public UserPasswordResetValidator(ITranslator translator) 
            : base(translator)
        {
            base.SetRules(this.AddRules); 
        }

        private void AddRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Translate("IdIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x.User)
                .NotEmpty()
                .WithMessage(Translate("UserIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage(Translate("TokenIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.RequestDate)
                .NotEmpty()
                .WithMessage(Translate("RequestDateIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.ExpirationDate)
                .NotEmpty()
                .WithMessage(Translate("ExpirationIsRequired"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);

            RuleFor(x => x.UseDate)
                .Empty()
                .WithMessage(Translate("UseDateMustBeEmpty"))
                .When(x => Scenario == Scenario.Create);
            RuleFor(x => x.UseDate)
                .NotEmpty()
                .WithMessage(Translate("UseDateIsRequired"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x.IsUsed)
                .Equal(false)
                .WithMessage(Translate("IsUsedMustBeFalse"))
                .When(x => Scenario == Scenario.Create);
            RuleFor(x => x.IsUsed)
                .Equal(true)
                .WithMessage(Translate("IsUsedMustBeTrue"))
                .When(x => Scenario == Scenario.Update);

            RuleFor(x => x.IsActive)
                .Equal(true)
                .WithMessage(Translate("IsActiveMustBeTrue"))
                .When(x => Scenario == Scenario.Create);
            RuleFor(x => x.IsActive)
                .Equal(false)
                .WithMessage(Translate("IsActiveMustBeFalse"))
                .When(x => Scenario == Scenario.Update);
        }
    }
}
