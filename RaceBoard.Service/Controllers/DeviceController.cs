using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Translations.Interfaces;
using RaceBoard.DTOs.Device.Request;
using RaceBoard.Domain;
using RaceBoard.DTOs.Device.Response;

namespace RaceBoard.Service.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : AbstractController<DeviceController>
    {
        private readonly IDeviceManager _deviceManager;
        private readonly IRequestContextHelper _requestContextHelper;

        public DeviceController
            (
                IMapper mapper,
                ILogger<DeviceController> logger,
                ITranslator translator,
                IDeviceManager deviceManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _deviceManager = deviceManager;
            _requestContextHelper = requestContextHelper;
        }

        [HttpPost()]
        public ActionResult<int> RegisterDevice([FromBody] DeviceRequest deviceRequest)
        {
            var device = _mapper.Map<Device>(deviceRequest);

            int id = _deviceManager.Register(device);

            return Ok(id);
        }

        [HttpGet("subscriptions")]
        public ActionResult<DeviceSubscriptionResponse> GetDeviceSubscription([FromQuery] int idDevice)
        {
            var device = new Device() {  Id = idDevice };

            var subscriptions = _deviceManager.GetSubscription(device);

            var response = _mapper.Map<DeviceSubscriptionResponse>(subscriptions);

            return Ok(response);
        }

        [HttpPost("subscriptions")]
        public ActionResult CreateDeviceSubscription([FromBody] DeviceSubscriptionRequest deviceSubscriptionRequest)
        {
            var subscription = _mapper.Map<DeviceSubscription>(deviceSubscriptionRequest);

            _deviceManager.CreateSubscription(subscription);

            return Ok();
        }

        [HttpDelete("subscriptions/{idDevice}")]
        public ActionResult DeleteDeviceSubscription([FromRoute] int idDevice)
        {
            _deviceManager.RemoveSubscription(idDevice);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}