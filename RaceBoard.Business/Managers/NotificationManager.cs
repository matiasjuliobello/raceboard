using Microsoft.Extensions.Configuration;
using RestSharp;
using Google.Apis.Auth.OAuth2;
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain.Notification;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class NotificationManager : AbstractManager, INotificationManager
    {
        private const string _GOOGLE_CREDENTIAL_SCOPE = "https://www.googleapis.com/auth/firebase.messaging";
        private const string _GOOGLE_FIREBASE_CREDENTIALS_FILE_PATH = "C:\\Matias.Bello\\Race Board\\SourceCode\\credentials.json";
        private const string _GOOGLE_FIREBASE_API_BASE_URL = "https://fcm.googleapis.com/v1";
        private const string _GOOGLE_FIREBASE_PROJECT_ID = "raceboard-b3663";

        #region Constructors

        public NotificationManager
            (
                ITranslator translator,
                IConfiguration configuration
            ) : base(translator)
        {
            //string googleCredentialsScope = configuration["Google_Credentials_Scope"];
            //string googleFirebaseCredentialsFilePath = configuration["Google_Firebase_Credentials_FilePath"];
            //string googleFirebaseApiBaseUrl = configuration["Google_Firebase_ApiBaseUrl"];
            //string googleFirebaseProjectId = configuration["Google_Firebase_ProjectId"];
        }

        #endregion

        #region INotificationManager implementation

        public string GetJwtToken()
        {
            var json = File.ReadAllText(_GOOGLE_FIREBASE_CREDENTIALS_FILE_PATH);

            var credentials = GoogleCredential.FromJson(json).CreateScoped(_GOOGLE_CREDENTIAL_SCOPE);

            return credentials.UnderlyingCredential.GetAccessTokenForRequestAsync().Result;
        }

        public async Task<RestResponse> SendNotification(Notification notification)
        {
            var firebaseNotification = new FirebaseNotification(notification.Title, notification.Message, notification.ImageFileUrl);

            if (notification.NotificationType == NotificationType.Message)
                firebaseNotification.message.token = notification.IdTarget;

            if (notification.NotificationType == NotificationType.Topic)
                firebaseNotification.message.topic = notification.IdTarget;

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

        #endregion
    }
}