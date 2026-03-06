using familytree_api.Dtos.AppSettings;
using familytree_api.Dtos.Emails;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace familytree_api.Services.Email
{
    public class EmailServices(
        IOptions<ZeptoMailConfig> zeptoConfig,
        IOptions<FrontEndUrl> frontEndURL,
        IWebHostEnvironment env,
        HttpClient httpClient) : IEmailServices
    {
        private readonly ZeptoMailConfig _config = zeptoConfig.Value;
        private readonly FrontEndUrl _frontEndURL = frontEndURL.Value;
        private readonly IWebHostEnvironment _env = env;
        private const string ApiUrl = "https://api.zeptomail.com/v1.1/email";

        private async Task SendAsync(string toEmail, string toName, string subject, string htmlBody)
        {
            var payload = new
            {
                from = new { address = _config.From, name = _config.FromName },
                to = new[]
        {
            new { email_address = new { address = toEmail, name = toName } }
        },
                subject,
                htmlbody = htmlBody
            };

            var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);
            request.Headers.TryAddWithoutValidation("Authorization", _config.Token);  // ← fix
            request.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"ZeptoMail response: {response.StatusCode} - {responseBody}");
            response.EnsureSuccessStatusCode();
        }

        private string LoadTemplate(string templateName)
        {
            var filePath = Path.Combine(
                _env.IsDevelopment() ? Directory.GetCurrentDirectory() : _env.ContentRootPath,
                "Dtos", "Emails", templateName
            );
            return File.ReadAllText(filePath);
        }

        public async Task SendEmailVerfication(EmailMessage email)
        {
            try
            {
                var html = LoadTemplate("WelcomeEmail.html")
                    .Replace("{{client}}", email.Name)
                    .Replace("{{validation_endpoint}}", $"{_frontEndURL.Url}/verify-email?token={email.ValidationToken}&email={email.To}");

                await SendAsync(email.To, email.Name, email.Subject, html);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        public async Task ResetPasswordEmail(EmailMessage email)
        {
            try
            {
                var html = LoadTemplate("ResetPasswordEmail.html")
                    .Replace("{{client}}", email.Name)
                    .Replace("{{url}}", $"{_frontEndURL.Url}/verify-password-reset?token={email.ValidationToken}&email={email.To}");

                await SendAsync(email.To, email.Name, email.Subject, html);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        public async Task InviteUser(EmailMessage email)
        {
            try
            {
                var html = LoadTemplate("Invitation.html")
                    .Replace("{{inviter}}", email.Inviter)
                    .Replace("{{email}}", email.To)
                    .Replace("{{password}}", email.Password)
                    .Replace("{{client}}", email.Name)
                    .Replace("{{login}}", _frontEndURL.Url);

                await SendAsync(email.To, email.Name, email.Subject, html);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        public async Task ChangeCredentials(EmailMessage email)
        {
            try
            {
                var html = LoadTemplate("CredentialsChange.html")
                    .Replace("{{inviter}}", email.Inviter)
                    .Replace("{{email}}", email.To)
                    .Replace("{{password}}", email.Password)
                    .Replace("{{client}}", email.Name)
                    .Replace("{{login}}", _frontEndURL.Url);

                await SendAsync(email.To, email.Name, email.Subject, html);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
    }
}