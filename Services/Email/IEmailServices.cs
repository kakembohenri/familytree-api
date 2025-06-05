using familytree_api.Dtos.Emails;
using System.Net.Mail;

namespace familytree_api.Services.Email
{
    public interface IEmailServices
    {
        Task SendEmailVerfication(EmailMessage email);
        Task ResetPasswordEmail(EmailMessage email);
        Task InviteUser(EmailMessage email);
        Task ChangeCredentials(EmailMessage email);
    }
}
