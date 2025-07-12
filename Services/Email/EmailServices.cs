using familytree_api.Dtos.AppSettings;
using familytree_api.Dtos.Emails;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace familytree_api.Services.Email
{
    public class EmailServices(IOptions<SmtpConfig> smtpConfig, IOptions<FrontEndUrl> frontEndURL, IWebHostEnvironment env, ILogger<EmailServices> logger) : IEmailServices
    {
        private readonly SmtpConfig _smtpConfig = smtpConfig.Value;
        private readonly FrontEndUrl _frontEndURL = frontEndURL.Value;
        private readonly IWebHostEnvironment _env = env;
        private readonly ILogger<EmailServices> _logger = logger;

             public async Task SendEmailVerfication(EmailMessage email)
        {
            try
            {
                using var smtp = new SmtpClient();

                SecureSocketOptions socketOption;
                if (_env.IsProduction())
                {
                    socketOption = _smtpConfig.EnableTls ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect;
                    _logger.LogInformation("Connecting to SMTP in Production mode with {SocketOption}", socketOption);
                }
                else
                {
                    socketOption = SecureSocketOptions.None;
                    _logger.LogInformation("Connecting to SMTP in Development mode (no encryption)");
                }

                await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, socketOption);

                if (_env.IsProduction())
                {
                    await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
                    _logger.LogInformation("Authenticated to SMTP as {Username}", _smtpConfig.UserName);
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpConfig.FromName, _smtpConfig.From));
                message.To.Add(new MailboxAddress(email.To, email.To));
                message.Subject = email.Subject;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Dtos", "Emails", "WelcomeEmail.html");
                if (!File.Exists(filePath))
                {
                    _logger.LogError("Email template not found at {Path}", filePath);
                    return;
                }

                var htmlContent = await File.ReadAllTextAsync(filePath);
                htmlContent = htmlContent.Replace("{{client}}", email.Name)
                                         .Replace("{{validation_endpoint}}", $"{_frontEndURL.Url}/verify-email?token={email.ValidationToken}&email={email.To}");

                message.Body = new TextPart("html") { Text = htmlContent };

                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {Recipient}", email.To);
            }
            catch (SmtpCommandException ex)
            {
                _logger.LogError(ex, "SMTP command error while sending email to {Recipient}. Status: {StatusCode}", email.To, ex.StatusCode);
            }
            catch (SmtpProtocolException ex)
            {
                _logger.LogError(ex, "SMTP protocol error while sending email to {Recipient}", email.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending email to {Recipient}", email.To);
            }
}

        public async Task ResetPasswordEmail(EmailMessage email)
        {
            try
            {

                using var smtp = new SmtpClient();

                // Connect to the Mailpit SMTP server
                if (_env.IsProduction())
                {
                    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, _smtpConfig.EnableTls ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect);
                    await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
                }
                else
                {
                    // Local development: use MailPit, Papercut, or log the email
                    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.None);
                }

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
                if (_env.IsProduction())
                {
                    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, _smtpConfig.EnableTls ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect);
                    await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
                }
                else
                {
                    // Local development: use MailPit, Papercut, or log the email
                    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.None);
                }

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
                if (_env.IsProduction())
                {
                    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, _smtpConfig.EnableTls ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect);
                    await smtp.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
                }
                else
                {
                    // Local development: use MailPit, Papercut, or log the email
                    await smtp.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, SecureSocketOptions.None);
                }

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
