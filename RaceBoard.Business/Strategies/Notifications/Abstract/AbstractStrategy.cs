using Microsoft.Extensions.Configuration;
using RaceBoard.Common.Exceptions;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;


namespace RaceBoard.Business.Strategies.Notifications.Abstract
{
    public abstract class AbstractStrategy
    {
        protected readonly string _baseUrl;
        private readonly ITranslator _translator;
        private readonly IMemberRepository _memberRepository;

        public AbstractStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IMemberRepository memberRepository
            )
        {
            _translator = translator;
            _memberRepository = memberRepository;
            _baseUrl = configuration["FrontEndUrl"];
        }

        protected List<Member> CheckForTargetMembers(Championship championship, IEnumerable<RaceClass> raceClasses)
        {
            var memberSearchFilter = new MemberSearchFilter()
            {
                Championship = championship,
                RaceClasses = raceClasses.ToArray()
            };

            var members = _memberRepository.Get(memberSearchFilter).Results.ToList();
            if (members.Count == 0)
                throw new FunctionalException(Common.Enums.ErrorType.ValidationError, "No target members found suitable for this notification");

            return members;
        }

        protected string BuildInvitationLink(string entityName, int entityId, Invitation invitation)
        {
            string url = Path.Combine(_baseUrl, "invitations");

            return $"<a href='{url}?join_{entityName.ToLower()}={entityId}&token={invitation.Token}'>{Translate("InvitationEmailLinkText")}</a>";
        }

        protected string BuildApplicationLink()
        {
            string url = Path.Combine(_baseUrl, "login");

            return $"<a href='{url}'>{Translate("LoginLinkText")}</a>";
        }

        protected string Translate(string text, params object[] arguments)
        {
            if (_translator == null)
                return text;

            return _translator.Get(text, arguments);
        }
    }
}
