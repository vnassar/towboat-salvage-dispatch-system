using MudBlazor;
using System.Collections.Concurrent;
using System.Net;
using RestSharp;

namespace TowBoatSalvageWebApp.Services
{
    public sealed class DispatchEmailService
    {
        private readonly MailgunEmailSender _sender;
        private readonly ILogger<DispatchEmailService> _logger;

        private readonly ISnackbar _snackbar;

        public DispatchEmailService(MailgunEmailSender sender, ILogger<DispatchEmailService> logger, ISnackbar Snackbar)
        {
            _sender = sender;
            _logger = logger;
            _snackbar = Snackbar;
        }

        public async Task SendAsync(
            string to,
            string subject,
            string text,
            byte[]? attachmentBytes = null,
            string? attachmentFileName = null,
            string attachmentContentType = "application/pdf",
            string? html = null,
            string? dispatchToken = null,
            CancellationToken cancellationToken = default
        )
        {
            const int maxRetries = 3;
            var delay = TimeSpan.FromSeconds(1);

            for (var attempt = 1; ; attempt++)
            {
                RestResponse response;

                try
                {
                    response = await _sender.SendAsync(
                    to,
                    subject,
                    text,
                    attachmentBytes,
                    attachmentFileName,
                    attachmentContentType,
                    html,
                    dispatchToken
                );
                }
                catch (Exception ex) when (attempt <= maxRetries)
                {
                    _logger.LogWarning(ex, "Mailgun send threw exception. To={To}, Attempt={Attempt}/{MaxRetries}. Retrying in {DelayMs}ms.", to, attempt, maxRetries, delay.TotalMilliseconds);

                    await Task.Delay(delay + TimeSpan.FromMilliseconds(Random.Shared.Next(100, 300)), cancellationToken);
                    delay = TimeSpan.FromSeconds(delay.TotalSeconds * 2);
                    continue;
                }

                if (response.IsSuccessful)
                {
                    _logger.LogInformation("Mailgun accepted message. To={To}, Status={StatusCode}", to, (int)response.StatusCode);
                    _snackbar.Add($"Dispatch email service: message successfully sent to: {to}.", Severity.Success);
                    return;
                }

                var statusCode = (int)response.StatusCode;
                var isTransient = response.StatusCode == HttpStatusCode.RequestTimeout || //408
                    statusCode == 429 || //too many request
                    (statusCode >= 500 && statusCode <= 599); //server errors

                if (isTransient && attempt <= maxRetries)
                {
                    _logger.LogWarning("Transient Mailgun failure. To={To}, Status={StatusCode}, Attempt={Attempt}/{MaxRetries}. Retrying in {DelayMs}ms. Content={Content}", to, statusCode, attempt, maxRetries, delay.TotalMilliseconds, response.Content);

                    await Task.Delay(delay + TimeSpan.FromMilliseconds(Random.Shared.Next(100, 300)), cancellationToken);
                    delay = TimeSpan.FromSeconds(delay.TotalSeconds * 2);
                    continue;
                }

                _logger.LogError(
                    "Mailgun send failed (non-retryable or retries exhausted). To={To}, Status={StatusCode}, Error={ErrorMessage}, Content={Content}",
                    to, statusCode, response.ErrorMessage, response.Content);

                throw new InvalidOperationException($"Mail send failed: {statusCode} {response.Content}");
            }
            
        }

        public async Task SendWithAttachmentAsync(
            string to,
            string subject,
            string body,
            string fileName,
            byte[] fileBytes,
            string contentType = "application/pdf")
        {
            var response = await _sender.SendAsync(
                to: to,
                subject: subject,
                text: body,
                attachmentBytes: fileBytes,
                attachmentFileName: fileName,
                attachmentContentType: contentType);

            if (!response.IsSuccessful)
            {
                throw new InvalidOperationException(
                    $"Mail send failed: {(int)response.StatusCode} {response.Content}");
            }
        }
    }
}