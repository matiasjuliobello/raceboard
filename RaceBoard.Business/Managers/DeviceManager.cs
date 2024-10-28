using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Data;
using RaceBoard.Data.Repositories;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using System;

namespace RaceBoard.Business.Managers
{
    public class DeviceManager : AbstractManager, IDeviceManager
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceSubscriptionRepository _deviceSubscriptionRepository;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public DeviceManager
            (
                IDeviceRepository deviceRepository,
                IDeviceSubscriptionRepository deviceSubscriptionRepository,
                ITranslator translator,
                IDateTimeHelper dateTimeHelper,
                IConfiguration configuration
            ) : base(translator)
        {
            _deviceRepository = deviceRepository;
            _deviceSubscriptionRepository = deviceSubscriptionRepository;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region IDeviceManager implementation

        public int Register(Device device, ITransactionalContext? context = null)
        {
            int id = 0;

            if (context == null)
                context = _deviceRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                DateTimeOffset now = _dateTimeHelper.GetCurrentTimestamp();

                device.LastUpdateDate = now;

                //if (!_deviceRepository.Exists(device.Token, context))
                var registeredDevice = _deviceRepository.Get(device.Token, context);
                if (registeredDevice == null)
                {
                    device.CreationDate = now;
                    id = _deviceRepository.Create(device, context);
                }
                else
                {
                    _deviceRepository.Update(device, context);
                    id = registeredDevice.Id;
                }

                _deviceRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _deviceRepository.CancelTransactionalContext(context);
                throw;
            }

            return id;
        }

        public DeviceSubscription GetSubscription(Device device)
        {
            var subscription = _deviceSubscriptionRepository.Get(device.Id);
            if (subscription == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return subscription;
        }

        public void CreateSubscription(DeviceSubscription deviceSubscription, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _deviceSubscriptionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                if (_deviceSubscriptionRepository.Exists(deviceSubscription, context))
                    _deviceSubscriptionRepository.Remove(deviceSubscription.Device.Id, context);

                _deviceSubscriptionRepository.Create(deviceSubscription, context);

                _deviceSubscriptionRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _deviceSubscriptionRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void RemoveSubscription(int idDevice, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _deviceSubscriptionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _deviceSubscriptionRepository.Remove(idDevice, context);

                _deviceSubscriptionRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _deviceSubscriptionRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}