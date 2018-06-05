using System.Net;
using System.Net.Mail;

public class MailManager : BitGameManager<MailManager>
{
    public void SendMail(string body)
    {
        var mail = new MailMessage
        {
            From = new MailAddress("wextia@gmail.com"),
            Subject = "Test Mail",
            Body = body,
        };

        mail.To.Add("wextia@gmail.com");

        var smtpServer = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials =
                new NetworkCredential("wextia", "kikolemono54"),
            EnableSsl = true
        };
        ServicePointManager.ServerCertificateValidationCallback =
            (s, certificate, chain, sslPolicyErrors) => true;

        smtpServer.SendAsync(mail, "");
    }
}