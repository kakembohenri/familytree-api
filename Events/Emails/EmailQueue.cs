using familytree_api.Dtos.Emails;
using System.Collections.Concurrent;

namespace familytree_api.Events.Emails
{
    public class EmailQueue: IEmailQueue
    {

        private readonly ConcurrentQueue<EmailMessage> _emails = new();
        private readonly SemaphoreSlim _signal = new(0);
        public void EnqueueEmail(EmailMessage email)
        {
            _emails.Enqueue(email);
            _signal.Release();
        }

        public async Task<EmailMessage?> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _emails.TryDequeue(out var email);
            return email;
        }
    }
}
