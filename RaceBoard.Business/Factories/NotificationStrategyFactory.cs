using Microsoft.Extensions.DependencyInjection;
using RaceBoard.Business.Strategies.Notifications.Email;
using RaceBoard.Business.Strategies.Notifications.Push;
using RaceBoard.Notification.Enums;
using RaceBoard.Notification.Interfaces;
using Email = RaceBoard.Business.Strategies.Notifications.Email;
using Push = RaceBoard.Business.Strategies.Notifications.Push;

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
                    NotificationType.Championship_Invitation,
                    new Type[]
                    {
                        typeof(Email.ChampionshipInvitationStrategy),
                        typeof(Push.ChampionshipInvitationStrategy)
                    }
                },
                //{
                //    NotificationType.TeamInvitation,
                //    new Type[]
                //    {
                //        typeof(Email.TeamInvitationStrategy) 
                //    }
                //}
            };
        }

        public IEnumerable<INotificationStrategy> ResolveStrategy(NotificationType notificationType)
        {
            try
            {
                var strategyTypes = strategies[notificationType];

                return _serviceProvider.GetServices<INotificationStrategy>().Where(x => strategyTypes.Contains(x.GetType()));
            }
            catch (Exception)
            {
                throw new NotImplementedException("EmailNotificationStrategyNotImplemented");
            }
        }
    }

}
