using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain.Notification;
using RaceBoard.Translations.Interfaces;
using RestSharp;

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
            var firebaseNotification = new FirebaseNotification(notification.DeviceToken, notification.Title, notification.Message, notification.ImageFileUrl);

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