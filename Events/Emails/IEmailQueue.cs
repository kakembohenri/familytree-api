using familytree_api.Dtos.Emails;

namespace familytree_api.Events.Emails
{
    public interface IEmailQueue
    {
        void EnqueueEmail(EmailMessage email);
        Task<EmailMessage?> DequeueAsync(CancellationToken token);
    }
}
