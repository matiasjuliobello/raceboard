using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using RaceBoard.Messaging.Interfaces;
using RaceBoard.Notification.Interfaces;
using RaceBoard.PushMessaging.Entities;
using RaceBoard.PushMessaging.Enums;
using RaceBoard.PushMessaging.Interfaces;
using RaceBoard.PushMessaging.Providers;
using RestSharp;

namespace RaceBoard.Messaging.Providers
{
    public class GoogleFirebasePushNotificationProvider : IPushNotificationProvider
    {
        private readonly string _googleCredentialsScope;
        private readonly string _googleFirebaseCredentialsFilePath;
        private readonly string _googleFirebaseApiBaseUrl;
        private readonly string _googleFirebaseProjectId;

        #region Constructors

        public GoogleFirebasePushNotificationProvider(IConfiguration configuration)
        {
            _googleCredentialsScope = configuration["Google_Credentials_Scope"];
            _googleFirebaseCredentialsFilePath = configuration["Google_Firebase_Credentials_FilePath"];
            _googleFirebaseApiBaseUrl = configuration["Google_Firebase_ApiBaseUrl"];
            _googleFirebaseProjectId = configuration["Google_Firebase_ProjectId"];
        }

        #endregion

        #region INotificationProvider implementation

        //public async Task<RestResponse> SendNotification(IMessagingPushNotification messagingNotification)
        public async Task Send(INotification notification)
        {
            var pushNotification = notification as PushNotification;
            if (pushNotification == null)
                throw new Exception("INotification implementation is not GoogleFirebaseNotification");

            var notificationData = pushNotification.Data as PushNotificationData;
            if (notificationData == null)
                throw new Exception("INotificationData implementation is not GoogleFirebaseNotification");

            PushNotificationType notificationType = notificationData.NotificationType;
            string idTarget = notificationData.IdTarget;
            string title = notificationData.Title;
            string message = notificationData.Message;
            string imageFileUrl = notificationData.ImageFileUrl;

            var firebaseNotification = new GoogleFirebasePushNotification(title, message, imageFileUrl);

            if (notificationType == PushNotificationType.Message)
                firebaseNotification.message.token = idTarget;

            if (notificationType == PushNotificationType.Topic)
                firebaseNotification.message.topic = idTarget;

            var bearerToken = GetJwtToken();

            var client = new RestClient(_googleFirebaseApiBaseUrl);
            var request = new RestRequest($"projects/{_googleFirebaseProjectId}/messages:send", Method.Post);

            request.AddHeader("Authorization", $"Bearer {bearerToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(firebaseNotification);

            var restResponse = await client.ExecuteAsync(request);
        }

        #endregion

        #region Private Methods

        private string GetJwtToken()
        {
            var json = File.ReadAllText(_googleFirebaseCredentialsFilePath);

            var credentials = GoogleCredential.FromJson(json).CreateScoped(_googleCredentialsScope);

            return credentials.UnderlyingCredential.GetAccessTokenForRequestAsync().Result;
        }

        #endregion
    }
}
