using Microsoft.Extensions.Configuration;
using RaceBoard.Business.Strategies.Notifications.Abstract;
using RaceBoard.Data.Repositories;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Mailing.Entities;
using RaceBoard.Notification.Interfaces;
using RaceBoard.Translations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceBoard.Business.Strategies.Notifications.Email
{
    public class ChampionshipFileUploadedStrategy : AbstractStrategy, INotificationStrategy
    {
        //private readonly IPersonRepository _personRepository;
        //private readonly IRoleRepository _roleRepository;
        //private readonly IChampionshipRepository _championshipRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly string _baseUrl;

        public ChampionshipFileUploadedStrategy
            (
                IConfiguration configuration,
                ITranslator translator,
                //IPersonRepository personRepository,
                //IRoleRepository roleRepository,
                //IChampionshipRepository championshipRepository,
                IMemberRepository memberRepository
            ) : base(configuration, translator)
        {
            //_personRepository = personRepository;
            //_roleRepository = roleRepository;
            //_championshipRepository = championshipRepository;
            _memberRepository = memberRepository;

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
            var memberSerachFilter = new MemberSearchFilter()
            {
                Championship = championshipFile.Championship,
                RaceClasses = championshipFile.RaceClasses.ToArray()
            };
            var members = _memberRepository.Get(memberSerachFilter).Results.ToList();

            string emailSubject = base.Translate("NewFileHasBeenUploaded");

            //string link = base.BuildInvitationLink("championship", championship!.Id, championshipMemberInvitation.Invitation);

            var emailHtmlContent = new StringBuilder();
            emailHtmlContent.AppendLine("<br />");
            emailHtmlContent.AppendLine($"New file has been uploaded to championship <b>'{championshipFile.Championship.Name}'</b>");
            emailHtmlContent.AppendLine("<br /><br /><br />");
            //emailHtmlContent.AppendLine(link);
            string emailBody = emailHtmlContent.ToString();

            string emails = String.Concat(members.Select(x => x.User.Email), ", ");

            string emailRecipientAddress = emails;
            string emailRecipientName = "";

            return new EmailNotificationData()
            {
                Subject = emailSubject,
                Body = emailBody,
                EmailAddress = emailRecipientAddress,
                FullName = emailRecipientName
            };
        }

        #endregion
    }
}
