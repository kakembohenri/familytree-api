using familytree_api.Dtos.Emails;

namespace familytree_api.Events.Emails
{
    public class PasswordResetEvent(IEmailQueue _emailQueue) : IEventHandler<PasswordReset>
    {
        public Task HandleAsync(PasswordReset @event)
        {
            var email = new EmailMessage
            {
                To = @event.To,
                Subject = @event.Subject,
                ValidationToken = @event.ValidationToken,
                Name = @event.Name,
                Action = Enums.EmailActions.ResetPassword,
                Password = @event.Password,
            };

            _emailQueue.EnqueueEmail(email);
            return Task.CompletedTask;
        }
    }
}
