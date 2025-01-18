using RaceBoard.Business.Validators.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;


namespace RaceBoard.Business.Validators
{
    public class EquipmentChangeRequestValidator : AbstractCustomValidator<EquipmentChangeRequest>
    {
        private readonly IEquipmentChangeRequestRepository _equipmentChangeRequestRepository;

        public EquipmentChangeRequestValidator
            (
                ITranslator translator,
                IEquipmentChangeRequestRepository equipmentChangeRequestRepository
            )
            : base(translator)
        {
            _equipmentChangeRequestRepository = equipmentChangeRequestRepository;

            base.SetRules(this.AddRules);
        }

        private void AddRules()
        {
        }
    }
}
