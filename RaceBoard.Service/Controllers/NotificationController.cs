using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Translations.Interfaces;
using RaceBoard.DTOs.Notification.Request;
using RaceBoard.Messaging.Interfaces;
using RaceBoard.Messaging.Entities;

namespace RaceBoard.Service.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : AbstractController<NotificationController>
    {
        private readonly INotificationProvider _notificationProvider;
        private readonly IRequestContextHelper _requestContextHelper;

        public NotificationController
            (
                IMapper mapper,
                ILogger<NotificationController> logger,
                ITranslator translator,
                INotificationProvider notificationProvider,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _requestContextHelper = requestContextHelper;
            _notificationProvider = notificationProvider;
        }

        [HttpPost("devices")]
        public async Task<ActionResult> SendNotificationToDevices(NotificationRequest notificationRequest)
        {
            //notificationRequest = new NotificationRequest()
            //{
            //   IdTarget = "eqvmtc1qSSeygajYwUqkXL:APA91bEv1PSvSXz12u6eHA2Xf98YTb7D3383sg-uw2LMlvTScJsdjOmpXAH0OYIwPforDmRfBPhxO5VE2AUk67GWFlMC_KsjpHY_E4bZ6TXmj-EzT9uPqybttSC_z11NSQ6CRGqh9VJJ",
            //    IdNotificationType = (int)NotificationType.Message,
            //    Title = "Esto es el título",
            //    Message = "Esto es el mensaje",
            //    ImageFileUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e6/Bandera_del_Club_de_Regatas.png"
            //};

            var notification = _mapper.Map<Notification>(notificationRequest);

            await _notificationProvider.SendNotification(notification);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}