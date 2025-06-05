using familytree_api.Dtos.Emails;

namespace familytree_api.Events.Emails
{
    public class EmailVerificationEvent(IEmailQueue _emailQueue) : IEventHandler<EmailMessage>
    {
        public Task HandleAsync(EmailMessage @event)
        {
            var email = new EmailMessage
            {
                To = @event.To,
                Subject = @event.Subject,
                ValidationToken = @event.ValidationToken,
                Name = @event.Name,
                Action = Enums.EmailActions.VerifyEmail
            };

            _emailQueue.EnqueueEmail(email);
            return Task.CompletedTask;
        }
    }
}
