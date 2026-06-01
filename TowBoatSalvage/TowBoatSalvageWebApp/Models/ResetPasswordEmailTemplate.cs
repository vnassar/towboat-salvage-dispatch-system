namespace TowBoatSalvageWebApp.Models
{
    public class ResetPasswordEmailTemplate
    {
        private const string LogoUrl = "https://towboatustb.com/assets/TowBoatLogo.png";

        public static string BuildPasswordResetLinkHtml(string userName, string resetLinkUrl)
        {
            return
                "<!DOCTYPE html>" +
                "<html lang='en'>" +
                "<head><meta charset='UTF-8'/></head>" +
                "<body style='margin:0;padding:0;background-color:#f4f4f4;font-family:Arial,sans-serif;'>" +
                "<table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f4f4f4;padding:30px 0;'>" +
                "<tr><td align='center'>" +
                "<table width='600' cellpadding='0' cellspacing='0' style='background-color:#ffffff;border-radius:6px;overflow:hidden;'>" +

                // Logo header
                "<tr><td align='center' style='padding:28px 40px 20px 40px;border-bottom:3px solid #1a4d8f;'>" +
                "<img src='" + LogoUrl + "' alt='TowBoatU.S.' width='220' style='display:block;'/>" +
                "</td></tr>" +

                // Body
                "<tr><td style='padding:32px 40px 24px 40px;'>" +
                "<p style='margin:0 0 16px 0;font-size:15px;color:#222222;'><strong>Dear " + userName + ",</strong></p>" +
                "<p style='margin:0 0 16px 0;font-size:15px;color:#444444;line-height:1.6;'>" +
                "TowBoatU.S Tampa Bay sent you this link to renew or reset your password" +
                "" +
                "</p>" +
                "<p style='margin:0 0 24px 0;font-size:15px;color:#444444;line-height:1.6;'>" +
                "" +
                "</p>" +

                // Button (table-based for Outlook compatibility)
                "<table cellpadding='0' cellspacing='0' style='margin:0 auto 24px auto;'>" +
                "<tr><td align='center' style='background-color:#1a4d8f;border-radius:5px;'>" +
                "<a href='" + resetLinkUrl + "' style='display:inline-block;padding:14px 36px;font-size:16px;font-weight:bold;color:#ffffff;text-decoration:none;'>" +
                "Reset Password" +
                "</a></td></tr></table>" +

                "<p style='margin:0 0 8px 0;font-size:13px;color:#888888;'>If the button does not work, copy and paste this link into your browser:</p>" +
                "<p style='margin:0 0 24px 0;font-size:13px;color:#1a4d8f;word-break:break-all;'>" +
                "<a href='" + resetLinkUrl + "' style='color:#1a4d8f;'>" + resetLinkUrl + "</a></p>" +

                "<p style='margin:0;font-size:15px;color:#444444;'><strong>Sincerely,</strong><br/>" +
                "<img src='" + LogoUrl + "' alt='TowBoatU.S.' width='100' style='margin-top:8px;display:block;'/></p>" +
                "</td></tr>" +

                // Footer
                "<tr><td style='background-color:#1a4d8f;padding:16px 40px;text-align:center;'>" +
                "<p style='margin:0;font-size:12px;color:#ffffff;'>Please do not reply to this auto-generated email.</p>" +
                "</td></tr>" +

                "</table></td></tr></table>" +
                "</body></html>";
        }
    }
}
