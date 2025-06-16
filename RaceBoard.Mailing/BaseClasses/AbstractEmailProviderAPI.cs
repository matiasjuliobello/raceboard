//using RaceBoard.Mailing.Enums;
//using RaceBoard.Mailing.Interfaces;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using System.Text;

//namespace RaceBoard.Mailing.BaseClasses
//{
//    public abstract class AbstractEmailProviderAPI : IEmailProvider
//    {
//        private HttpClient _httpClient;

//        protected string _apiKey = string.Empty;
//        protected string _baseUrl = string.Empty;
//        protected string _resourceUriSingleDelivery = string.Empty;
//        protected string _resourceUriBulkDelivery = string.Empty;
        
//        private string _authHeaderName = string.Empty;
//        private string _authHeaderPrefix = string.Empty;

//        protected IEmailAddress _sender;
//        protected List<IEmailAddress> _recipients = new List<IEmailAddress>();
//        protected List<IEmailAttachment> _attachments = new List<IEmailAttachment>();

//        protected List<string> _subjects = new List<string>();
//        protected List<string> _bodies = new List<string>();

//        protected object _content;

//        protected EmailDeliveryType _deliveryType;

//        public AbstractEmailProviderAPI(IConfiguration configuration)
//        {
//            _apiKey = ReadValueFromConfiguration(configuration, "EmailProvider_API_Key");
//            _baseUrl = ReadValueFromConfiguration(configuration, "EmailProvider_URL_Base");
//            _resourceUriSingleDelivery = ReadValueFromConfiguration(configuration, "EmailProvider_URL_ResourceSingleDelivery");
//            _resourceUriBulkDelivery = ReadValueFromConfiguration(configuration, "EmailProvider_URL_ResourceBulkDelivery");

//            _authHeaderName = ReadValueFromConfiguration(configuration,   "EmailProvider_API_Header_Authentication_Name");
//            _authHeaderPrefix = ""; // ReadValueFromConfiguration(configuration, "EmailProvider_API_Header_Auth_Prefix");

//            _httpClient = new HttpClient();
//            _httpClient.BaseAddress = new Uri(_baseUrl);

//            SetAuthorization(_apiKey, _authHeaderName, _authHeaderPrefix);
//        }

//        public void SetAuthorization(string apiKey, string headerName = "Authorization", string apiKeyPrefix = "Bearer ")
//        {
//            _httpClient.DefaultRequestHeaders.Add(headerName, $"{apiKeyPrefix}{_apiKey}");
//        }

//        private string ReadValueFromConfiguration(IConfiguration configuration, string key)
//        {
//            string value = configuration[key];

//            if (value == null)
//                throw new ArgumentNullException($"Configuration value '{key}' is missing.");

//            return value;
//        }

//        public virtual void AddSender(IEmailAddress mailAddress)
//        {
//            _sender = mailAddress;
//        }

//        public virtual void AddRecipient(IEmailAddress mailAddress)
//        {
//            _recipients.Add(mailAddress);
//        }
//        public virtual void AddRecipients(IEnumerable<IEmailAddress> mailAddresses)
//        {
//            _recipients.AddRange(mailAddresses);
//        }

//        public virtual void AddAttachments(IEnumerable<IEmailAttachment> attachments)
//        {
//            _attachments.AddRange(attachments);
//        }

//        public virtual void AddSubject(string value)
//        {
//            _subjects.Add(value);
//        }

//        public virtual void AddBody(string value)
//        {
//            _bodies.Add(value);
//        }

//        public abstract void PrepareSend(EmailDeliveryType emailDeliveryType);

//        public async Task SendAsync()
//        {
//            await Task.Run(() => Send());
//        }

//        public void Send()
//        {
//            // bulk-email
//            if (_content == null)
//            {
//                throw new Exception("Content is null. Most likely due to PrepareSend() method not been invoked previously.");
//            }

//            string resourceUri = "";
            
//            switch(_deliveryType)
//            {
//                case EmailDeliveryType.Single:
//                    resourceUri = _resourceUriSingleDelivery;
//                    break;
//                case EmailDeliveryType.Bulk:
//                    resourceUri = _resourceUriBulkDelivery;
//                    break;
//                case EmailDeliveryType.Unspecified:
//                    throw new Exception("DeliveryType has not been set.");
//            }

//            var serializerSettings = new JsonSerializerSettings
//            {
//                ContractResolver = new CamelCasePropertyNamesContractResolver()
//            };

//            string jsonContent = JsonConvert.SerializeObject(_content, serializerSettings);

//            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

//            var response = _httpClient.PostAsync(resourceUri, stringContent).Result;
//        }
//    }
//}
