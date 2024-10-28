using Newtonsoft.Json;

namespace RaceBoard.Domain.Notification
{
    public class AndroidContent
    {
        public AndroidNotificationContent notification { get; set; }
    }

    public class AndroidNotificationContent
    {
        public string image { get; set; }
    }

    public class AppleContent
    {
        public AppleNotificationPayload payload { get; set; }
        public AppleNotificationContent fcm_options { get; set; }
    }

    public class AppleNotificationContent
    {
        public string image { get; set; }
    }

    public class AppleNotificationPayload
    {
        public AppleNotificationPayloadContent aps { get; set; }
    }

    public class AppleNotificationPayloadContent
    {
        [JsonProperty("mutable-content")]
        public int mutablecontent { get; set; }
    }

    public class NotificationContent
    {
        public string body { get; set; }
        public string title { get; set; }
    }

    public class Message
    {
        public string token { get; set; }
        public NotificationContent notification { get; set; }
        public AndroidContent android { get; set; }
        public AppleContent apns { get; set; }
    }

    public class FirebaseNotification
    {
        public Message message { get; set; }

        public FirebaseNotification(string deviceToken, string title, string messag, string? imageFileUrl = null)
        {
            this.message = new Message()
            {
                token = deviceToken,
                notification = new NotificationContent()
                {
                    title = title,
                    body = messag
                }
            };

            if (!String.IsNullOrEmpty(imageFileUrl))
            {
                this.message.android = new AndroidContent()
                {
                    notification = new AndroidNotificationContent()
                    {
                        image = imageFileUrl
                    }
                };

                this.message.apns = new AppleContent()
                {
                    payload = new AppleNotificationPayload()
                    {
                        aps = new AppleNotificationPayloadContent()
                        {
                            mutablecontent = 1
                        }
                    },
                    fcm_options = new AppleNotificationContent()
                    {
                        image = imageFileUrl
                    }
                };
            }
        }
    }
}
