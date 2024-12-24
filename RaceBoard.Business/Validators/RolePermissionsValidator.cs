using FluentValidation;
using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Enums;

namespace RaceBoard.Business.Validators
{
    public class RolePermissionsValidator : AbstractCustomValidator<RolePermissions>
    {
        public RolePermissionsValidator
            (
                ITranslator translator
            )
            : base(translator)
        {
            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
            RuleFor(x => x)
                .Must(x => x.Permissions.GroupBy(x => x.Action.Id).Count() == 1)
                .WithMessage(Translate("IdActionCannotBeRepeated"))
                .When(x => Scenario == Scenario.Create || Scenario == Scenario.Update);
        }
    }
}