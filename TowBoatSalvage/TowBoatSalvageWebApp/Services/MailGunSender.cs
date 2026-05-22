using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using RestSharp; // RestSharp v112.1.0
using RestSharp.Authenticators;
using System.Threading;
using System.Threading.Tasks;

namespace TowBoatSalvageWebApp.Services
{
    public sealed class MailgunEmailSender
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly string _domain;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly string _baseUrl;

        public MailgunEmailSender(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["Mailgun:ApiKey"] ?? "";
            _domain = config["Mailgun:Domain"] ?? "";
            _fromEmail = config["Mailgun:FromEmail"] ?? "no-reply@yourdomain.com";
            _fromName = config["Mailgun:FromName"] ?? "TowBoatUS";
            var region = (config["Mailgun:Region"] ?? "US").ToUpperInvariant();
            _baseUrl = region == "US" ? "https://api.mailgun.net/v3" : "https://api.mailgun.net/v3";
        }

        public async Task<RestResponse> SendAsync(
            string to,
            string subject,
            string text,
            byte[]? attachmentBytes = null,
            string? attachmentFileName = null,
            string attachmentContentType = "application/pdf",
            string? html = null,
            string? dispatchToken = null)
        {
            if (string.IsNullOrWhiteSpace(_apiKey) || string.IsNullOrWhiteSpace(_domain)) throw new InvalidOperationException("Mailgun configuration is missing Apikey or Domain");


            var client = new RestClient(new RestClientOptions($"https://api.mailgun.net/v3/{_domain}")
            {
                Authenticator = new HttpBasicAuthenticator("api", _apiKey)
            });

            var request = new RestRequest("messages", Method.Post)
            {
                AlwaysMultipartFormData = true
            };

            request.AddParameter("from", $"{_fromName} <{ _fromEmail}>");
            request.AddParameter("to", to);
            request.AddParameter("subject", subject);
            request.AddParameter("text", text);

            if(!string.IsNullOrWhiteSpace(html))
            {
                request.AddParameter("html", html);
            }

            request.AddParameter("o:tracking", "yes");
            request.AddParameter("o:tracking-opens", "yes");
            request.AddParameter("o:tracking-clicks", "yes");

            if (!string.IsNullOrWhiteSpace(dispatchToken))
            {
                request.AddParameter("v:dispatch_token", dispatchToken);
            }

            if (attachmentBytes is not null && attachmentBytes.Length > 0)
            {
                request.AddFile(
                    name: "attachment",
                    bytes: attachmentBytes,
                    fileName: attachmentFileName ?? "signed-document.pdf",
                    contentType: attachmentContentType);
            }

            return await client.ExecuteAsync(request);
        }

        public async Task SendAsyncOriginal(string toEmail, string subject, string plainText, byte[]? attachmentBytes = null, string? attachmentFileName = null, string attachmentContentType = "application/pdf")
        {
            if (string.IsNullOrWhiteSpace(_apiKey) ||
                string.IsNullOrWhiteSpace(_domain) ||
                string.IsNullOrWhiteSpace(toEmail))
            {
                return;
            }

            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{_apiKey}"));
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("from", $"{_fromName} <{_fromEmail}>"),
                new KeyValuePair<string, string>("to", toEmail),
                new KeyValuePair<string, string>("subject", subject),
                new KeyValuePair<string, string>("text", plainText)
            });

            var response = await _http.PostAsync($"{_baseUrl}/{_domain}/messages", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<RestResponse> SendTestEmail()
        {
            var options = new RestClientOptions("https://api.mailgun.net")
            {
                Authenticator = new HttpBasicAuthenticator("api", Environment.GetEnvironmentVariable("api_key") ?? "api_key")
            };

            var client = new RestClient(options);
            var request = new RestRequest("/v3/towboatustb.com/messages", Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("from", "TowBoatUs Tampa Bay <postmaster@towboatustb.com>");
            request.AddParameter("to", "Victor Nassar <vctrnssr@gmail.com>");
            request.AddParameter("subject", "Hello Victor Nassar");
            request.AddParameter("text", "Congratulations Victor Nassar, you just sent an email with Mailgun! You are truly awesome!");

            
            return await client.ExecuteAsync(request);
        }
    }
    
}