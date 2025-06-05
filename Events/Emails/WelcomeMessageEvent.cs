using familytree_api.Dtos.Emails;

namespace familytree_api.Events.Emails
{
    public class WelcomeMessageEvent(IEmailQueue _emailQueue) : IEventHandler<UserInvite>
    {
        public Task HandleAsync(UserInvite @event)
        {
            var email = new EmailMessage
            {
                To = @event.To,
                Subject = @event.Subject,
                Name = @event.Name,
                Action = Enums.EmailActions.Welcome,
                Password = @event.Password,
                Inviter = @event.Inviter,
                InviterEmail = @event.InviterEmail,
            };

            _emailQueue.EnqueueEmail(email);
            return Task.CompletedTask;
        }
    }
}
