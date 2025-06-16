using Microsoft.Extensions.Configuration;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;


namespace RaceBoard.Business.Strategies.Notifications.Abstract
{
    public abstract class AbstractStrategy
    {
        private readonly string _baseUrl;
        private readonly ITranslator _translator;

        public AbstractStrategy
            (
                IConfiguration configuration,
                ITranslator translator
            )
        {
            _translator = translator;

            _baseUrl = Path.Combine(configuration["FrontEndUrl"], "invitations");
        }


        protected string BuildInvitationLink(string entityName, int entityId, Invitation invitation)
        {
            return $"<a href='{_baseUrl}?join_{entityName}={entityId}&token={invitation.Token}'>{Translate("InvitationEmailLinkText")}</a>";
        }

        protected string Translate(string text, params object[] arguments)
        {
            if (_translator == null)
                return text;

            return _translator.Get(text, arguments);
        }
    }
}
