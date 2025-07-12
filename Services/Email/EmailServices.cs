using familytree_api.Dtos.AppSettings;
using familytree_api.Dtos.Emails;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace familytree_api.Services.Email
{
    public class EmailServices(IOptions<SmtpConfig> smtpConfig, IOptions<FrontEndUrl> frontEndURL, IWebHostEnvironment env) : IEmailServices
    {
        private readonly SmtpConfig _smtpConfig = smtpConfig.Value;
        private readonly FrontEndUrl _frontEndURL = frontEndURL.Value;
        private readonly IWebHostEnvironment _env = env;

        public async Task SendEmailVerfication(EmailMessage email)
        {
            try
            {
                using var smtp = new SmtpClient();

                // Connect to the Mailpit SMTP server
                //if (_env.IsProduction())
                //{
                //    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.SslOnConnect);
                //    await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
                //}
                //else
                //{
                //    // Local development: use MailPit, Papercut, or log the email
                //    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.None);
                //}

                await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.SslOnConnect);
                    await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);

                // Create the email message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpConfig.FromName, _smtpConfig.From));
                message.To.Add(new MailboxAddress(email.To, email.To));
                message.Subject = email.Subject;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Dtos", "Emails", "WelcomeEmail.html");
                // Read the HTML template
                var htmlContent = File.ReadAllText(filePath);

                // Replace placeholders with dynamic content
                htmlContent = htmlContent.Replace("{{client}}", email.Name); // Replace with actual user name
                htmlContent = htmlContent.Replace("{{validation_endpoint}}", $"{_frontEndURL.Url}/verify-email?token={email.ValidationToken}&email={email.To}"); // Replace with actual link

                // Set the email body to the HTML content
                message.Body = new TextPart("html") { Text = htmlContent };

                // Send the email
                await smtp.SendAsync(message);

                // Disconnect from the SMTP server
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task ResetPasswordEmail(EmailMessage email)
        {
            try
            {

                using var smtp = new SmtpClient();

                // Connect to the Mailpit SMTP server
                //if (_env.IsProduction())
                //{
                //    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.SslOnConnect);
                //    await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
                //}
                //else
                //{
                //    // Local development: use MailPit, Papercut, or log the email
                //    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.None);
                //}


                await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);

                // Create the email message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpConfig.FromName, _smtpConfig.From));
                message.To.Add(new MailboxAddress(email.To, email.To));
                message.Subject = email.Subject;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Dtos", "Emails", "ResetPasswordEmail.html");
                // Read the HTML template
                var htmlContent = File.ReadAllText(filePath);

                // Replace placeholders with dynamic content
                htmlContent = htmlContent.Replace("{{client}}", email.Name); // Replace with actual user name
                htmlContent = htmlContent.Replace("{{url}}", $"{_frontEndURL.Url}/verify-password-reset?token={email.ValidationToken}&email={email.To}"); // Replace with actual link

                // Set the email body to the HTML content
                message.Body = new TextPart("html") { Text = htmlContent };

                // Send the email
                await smtp.SendAsync(message);

                // Disconnect from the SMTP server
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task InviteUser(EmailMessage email)
        {
            try
            {

                using var smtp = new SmtpClient();

                // Connect to the Mailpit SMTP server
                //if (_env.IsProduction())
                //{
                //    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.SslOnConnect);
                //    await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
                //}
                //else
                //{
                //    // Local development: use MailPit, Papercut, or log the email
                //    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.None);
                //}


                await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);

                // Create the email message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpConfig.FromName, _smtpConfig.From));
                message.To.Add(new MailboxAddress(email.To, email.To));
                message.Subject = email.Subject;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Dtos", "Emails", "Invitation.html");
                // Read the HTML template
                var htmlContent = File.ReadAllText(filePath);

                // Replace placeholders with dynamic content
                htmlContent = htmlContent.Replace("{{inviter}}", email.Inviter); // Replace with actual the inviter
                htmlContent = htmlContent.Replace("{{email}}", email.To); // Replace with invited users email
                htmlContent = htmlContent.Replace("{{password}}", email.Password); // Replace with actual user name
                htmlContent = htmlContent.Replace("{{client}}", email.Name); // Replace with actual user name
                htmlContent = htmlContent.Replace("{{login}}", $"{_frontEndURL.Url}"); // Replace with actual login link

                // Set the email body to the HTML content
                message.Body = new TextPart("html") { Text = htmlContent };

                // Send the email
                await smtp.SendAsync(message);

                // Disconnect from the SMTP server
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task ChangeCredentials(EmailMessage email)
        {
            try
            {

                using var smtp = new SmtpClient();

                // Connect to the Mailpit SMTP server
                //if (_env.IsProduction())
                //{
                //    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.SslOnConnect);
                //    await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
                //}
                //else
                //{
                //    // Local development: use MailPit, Papercut, or log the email
                //    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.None);
                //}


                await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);

                // Create the email message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpConfig.FromName, _smtpConfig.From));
                message.To.Add(new MailboxAddress(email.To, email.To));
                message.Subject = email.Subject;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Dtos", "Emails", "CredentialsChange.html");
                // Read the HTML template
                var htmlContent = File.ReadAllText(filePath);

                // Replace placeholders with dynamic content
                htmlContent = htmlContent.Replace("{{inviter}}", email.Inviter); // Replace with actual the inviter
                htmlContent = htmlContent.Replace("{{email}}", email.To); // Replace with invited users email
                htmlContent = htmlContent.Replace("{{password}}", email.Password); // Replace with actual user name
                htmlContent = htmlContent.Replace("{{client}}", email.Name); // Replace with actual user name
                htmlContent = htmlContent.Replace("{{login}}", $"{_frontEndURL.Url}"); // Replace with actual login link

                // Set the email body to the HTML content
                message.Body = new TextPart("html") { Text = htmlContent };

                // Send the email
                await smtp.SendAsync(message);

                // Disconnect from the SMTP server
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
