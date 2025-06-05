using familytree_api.Dtos.Emails;

namespace familytree_api.Events.Emails
{
    public class CredentialsChangeEvent(IEmailQueue _emailQueue) : IEventHandler<CredentialsChange>
    {
        public Task HandleAsync(CredentialsChange @event)
        {
            var email = new EmailMessage
            {
                To = @event.To,
                Subject = @event.Subject,
                ValidationToken = @event.ValidationToken,
                Name = @event.Name,
                Action = Enums.EmailActions.CredentialChange,
                 Password = @event.Password,
                Inviter = @event.Inviter,
                InviterEmail = @event.InviterEmail,
            };

            _emailQueue.EnqueueEmail(email);
            return Task.CompletedTask;
        }
    }
}
