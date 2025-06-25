using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Data.Repositories;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Mailing.Entities;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Translations.Interfaces;
using System.Data;
using System.Text;


namespace RaceBoard.Business.Strategies.Notifications.Email
{
    public class ChampionshipFileUploadStrategy : AbstractStrategy, INotificationStrategy
    {
        private readonly IChampionshipRepository _championshipRepository;
        //private readonly IMemberRepository _memberRepository;
        private readonly IPersonRepository _personRepository;
        private readonly string _baseUrl;

        public ChampionshipFileUploadStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                IChampionshipRepository championshipRepository,
                IMemberRepository memberRepository,
                IPersonRepository personRepository
            ) : base(configuration, translator, memberRepository)
        {
            _championshipRepository = championshipRepository;
            //_memberRepository = memberRepository;
            _personRepository = personRepository;

            _baseUrl = Path.Combine(configuration["FrontEndUrl"], "invitations");
        }

        public INotification Produce(object data)
        {
            var notificationData = this.BuildNotificationData(data as ChampionshipFile);

            return new EmailNotification()
            {
                Media = Notification.Enums.NotificationMedia.Mail,
                //Settings = new EmailNotificationSettings(),
                Data = notificationData
            };
        }

        #region Private Methods

        private EmailNotificationData BuildNotificationData(ChampionshipFile championshipFile)
        {
            var championship = _championshipRepository.Get(championshipFile.Championship.Id);
            if (championship == null)
                throw new Exception($"No championship was found with ID = {championshipFile.Championship.Id}");

            var members = base.CheckForTargetMembers(championshipFile.Championship, championshipFile.RaceClasses);

            var requestPerson = _personRepository.GetByIdUser(championshipFile.File.CreationUser.Id);

            string subject = base.Translate("ChampionshipNewFileUploadEmailSubject");
            string body = String.Format(base.Translate("ChampionshipNewFileUploadEmailSubjectEmailBody"), requestPerson.Fullname, championshipFile.File.Name, championship.Name);

            string recipientAddress = String.Join(", ", members.Select(x => x.User.Email));
            string recipientName = "";

            return new EmailNotificationData()
            {
                Subject = subject,
                Body = body,
                EmailAddress = recipientAddress,
                FullName = recipientName
            };
        }

        #endregion
    }
}
