using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Translations.Interfaces;
using RaceBoard.DTOs.Notification.Request;
using RaceBoard.Messaging.Interfaces;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.PushMessaging.Entities;

namespace RaceBoard.Service.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : AbstractController<NotificationController>
    {
        private readonly IPushNotificationProvider _pushNotificationProvider;
        private readonly IRequestContextManager _requestContextManager;

        public NotificationController
            (
                IMapper mapper,
                ILogger<NotificationController> logger,
                ITranslator translator,
                IPushNotificationProvider pushNotificationProvider,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _requestContextManager = requestContextManager;
            _pushNotificationProvider = pushNotificationProvider;
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

            var notification = _mapper.Map<PushNotification>(notificationRequest);

            await _pushNotificationProvider.Send(notification);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}