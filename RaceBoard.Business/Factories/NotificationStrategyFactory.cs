using Microsoft.Extensions.DependencyInjection;
using RaceBoard.Notification.Enums;
using RaceBoard.Notification.Interfaces;
using Email = RaceBoard.Business.Strategies.Notifications.Email;
using Push  = RaceBoard.Business.Strategies.Notifications.Push;

namespace RaceBoard.Business.Factories
{
    public class NotificationStrategyFactory : INotificationStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<NotificationType, Type[]> strategies;

        public NotificationStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            strategies = new Dictionary<NotificationType, Type[]>
            {
                {
                    NotificationType.User_Creation,
                    new Type[]
                    {
                        typeof(Email.UserCreationStrategy)
                    }
                },
                {
                    NotificationType.Organization_Creation,
                    new Type[]
                    {
                        typeof(Email.OrganizationCreationStrategy),
                        //typeof(Push.OrganizationCreatedStrategy)
                    }
                },
                {
                    NotificationType.Organization_Member_Invitation,
                    new Type[]
                    {
                        typeof(Email.OrganizationMemberInvitationStrategy),
                        //typeof(Push.OrganizationMemberInvitationStrategy)
                    }
                },
                {
                    NotificationType.Championship_Creation,
                    new Type[]
                    {
                        typeof(Email.ChampionshipCreationStrategy),
                        //typeof(Push.ChampionshipCreationStrategy)
                    }
                },
                {
                    NotificationType.Championship_Member_Invitation,
                    new Type[]
                    {
                        typeof(Email.ChampionshipMemberInvitationStrategy),
                        typeof(Push.ChampionshipMemberInvitationStrategy)
                    }
                },
                {
                    NotificationType.Championship_File_Upload,
                    new Type[]
                    {
                        typeof(Email.ChampionshipFileUploadStrategy),
                        typeof(Push.ChampionshipFileUploadStrategy)
                    }
                },
                {
                    NotificationType.Team_Creation,
                    new Type[]
                    {
                        typeof(Email.TeamCreationStrategy),
                        //typeof(Push.TeamCreatedStrategy)
                    }
                },
                {
                    NotificationType.Team_Member_Invitation,
                    new Type[]
                    {
                        typeof(Email.TeamMemberInvitationStrategy),
                        //typeof(Push.TeamMemberInvitationStrategy)
                    }
                },
            };
        }

        public IEnumerable<INotificationStrategy> ResolveStrategy(NotificationType notificationType)
        {
            try
            {
                var strategyTypes = strategies[notificationType];

                return _serviceProvider.GetServices<INotificationStrategy>().Where(x => strategyTypes.Contains(x.GetType()));
            }
            catch (Exception e)
            {
                throw new NotImplementedException("NotificationStrategyNotImplemented");
            }
        }
    }

}
