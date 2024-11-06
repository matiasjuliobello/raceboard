using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Translations.Interfaces;
using RaceBoard.DTOs.Notification.Request;
using RaceBoard.Domain.Notification;

namespace RaceBoard.Service.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : AbstractController<NotificationController>
    {
        private readonly INotificationManager _userDeviceManager;
        private readonly IRequestContextHelper _requestContextHelper;

        public NotificationController
            (
                IMapper mapper,
                ILogger<NotificationController> logger,
                ITranslator translator,
                INotificationManager userDeviceManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _userDeviceManager = userDeviceManager;
            _requestContextHelper = requestContextHelper;
        }

        [HttpPost("devices")]
        public ActionResult SendNotificationToDevices(NotificationRequest notificationRequest)
        {
            var notification = _mapper.Map<Notification>(notificationRequest);

            //string deviceToken = "eqvmtc1qSSeygajYwUqkXL:APA91bEv1PSvSXz12u6eHA2Xf98YTb7D3383sg-uw2LMlvTScJsdjOmpXAH0OYIwPforDmRfBPhxO5VE2AUk67GWFlMC_KsjpHY_E4bZ6TXmj-EzT9uPqybttSC_z11NSQ6CRGqh9VJJ";
            //string title = "HOLAA";
            //string message = "qué hay de nuevo, viejo?";
            //string imageFileUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e6/Bandera_del_Club_de_Regatas.png";

            _userDeviceManager.SendNotification(notification);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}