using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using RaceBoard.Messaging.Entities;
using RaceBoard.Messaging.Interfaces;
using RestSharp;

namespace RaceBoard.Messaging.Providers
{
    public class GoogleFirebaseNotificationProvider : INotificationProvider
    {
        private const string _GOOGLE_CREDENTIAL_SCOPE = "https://www.googleapis.com/auth/firebase.messaging";
        private const string _GOOGLE_FIREBASE_CREDENTIALS_FILE_PATH = "C:\\Matias.Bello\\Race Board\\SourceCode\\credentials.json";
        private const string _GOOGLE_FIREBASE_API_BASE_URL = "https://fcm.googleapis.com/v1";
        private const string _GOOGLE_FIREBASE_PROJECT_ID = "raceboard-b3663";

        private readonly bool _enabled;

        #region Constructors

        public GoogleFirebaseNotificationProvider(IConfiguration configuration)
        {
            //string googleCredentialsScope = configuration["Google_Credentials_Scope"];
            //string googleFirebaseCredentialsFilePath = configuration["Google_Firebase_Credentials_FilePath"];
            //string googleFirebaseApiBaseUrl = configuration["Google_Firebase_ApiBaseUrl"];
            //string googleFirebaseProjectId = configuration["Google_Firebase_ProjectId"];

            bool.TryParse(configuration["Messaging_Enabled"], out _enabled);
        }

        #endregion

        #region INotificationProvider implementation

        public async Task<RestResponse> SendNotification(IMessagingNotification messagingNotification)
        {
            if (!_enabled)
                return await Task.FromResult<RestResponse>(null);

            var notification = messagingNotification as Notification;
            if (notification == null)
                throw new Exception("INotification implementation is not GoogleFirebaseNotification");

            NotificationType notificationType = notification.NotificationType;
            string idTarget = notification.IdTarget;
            string title = notification.Title;
            string message = notification.Message;
            string imageFileUrl = notification.ImageFileUrl;

            var firebaseNotification = new GoogleFirebaseNotification(title, message, imageFileUrl);

            if (notificationType == NotificationType.Message)
                firebaseNotification.message.token = idTarget;

            if (notificationType == NotificationType.Topic)
                firebaseNotification.message.topic = idTarget;

            var bearerToken = GetJwtToken();

            var client = new RestClient(_GOOGLE_FIREBASE_API_BASE_URL);
            var request = new RestRequest($"projects/{_GOOGLE_FIREBASE_PROJECT_ID}/messages:send", Method.Post);

            request.AddHeader("Authorization", $"Bearer {bearerToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(firebaseNotification);

            return await client.ExecuteAsync(request);
        }

        #endregion

        #region Private Methods

        private string GetJwtToken()
        {
            var json = File.ReadAllText(_GOOGLE_FIREBASE_CREDENTIALS_FILE_PATH);

            var credentials = GoogleCredential.FromJson(json).CreateScoped(_GOOGLE_CREDENTIAL_SCOPE);

            return credentials.UnderlyingCredential.GetAccessTokenForRequestAsync().Result;
        }

        #endregion
    }
}
