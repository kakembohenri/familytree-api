using familytree_api.Events.Emails;
using familytree_api.Services.Email;
using System;

namespace familytree_api.Workers.Emails
{
    public class EmailWorker(IEmailQueue _emailQueue, IServiceProvider _serviceProvider) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var email = await _emailQueue.DequeueAsync(stoppingToken);
                if (email != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var emailSender = scope.ServiceProvider.GetRequiredService<IEmailServices>();
                    if (email.Action == Enums.EmailActions.VerifyEmail)
                    {
                        await emailSender.SendEmailVerfication(email);
                    }else if (email.Action == Enums.EmailActions.ResetPassword) 
                    {
                        await emailSender.ResetPasswordEmail(email);
                    }else if(email.Action == Enums.EmailActions.Welcome)
                    {
                        await emailSender.InviteUser(email);
                    }else if (email.Action == Enums.EmailActions.CredentialChange)
                    {
                        await emailSender.ChangeCredentials(email);
                    }
                }
            }
        }
    }
}
