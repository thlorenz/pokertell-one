//Date: 6/5/2009

namespace PokerTell.User.Reporting
{
    using System.IO;
    using System.Net;
    using System.Net.Mail;

    using Interfaces;

    /// <summary>
    /// Description of Emailer.
    /// </summary>
    public class Emailer : IEmailer
    {
        const string password = "hhdeluxe";

        const int port = 587;

        const string server = "smtp.gmail.com";

        const string userName = "pokertellreports@gmail.com";

        public void Send(string subject, string body)
        {
            PrepareClient().Send(PrepareMailMessage(subject, body));
        }

        public void Send(string subject, string body, FileInfo[] attachments)
        {
            MailMessage msg = PrepareMailMessage(subject, body);
            var copyOfFiles = (FileInfo[])attachments.Clone();

            foreach (FileInfo fi in copyOfFiles)
            {
                if (File.Exists(fi.FullName))
                {
                    var attach = new Attachment(fi.FullName);
                    msg.Attachments.Add(attach);
                }
            }

            PrepareClient().Send(msg);
        }

        static SmtpClient PrepareClient()
        {
            return new SmtpClient(server, port) { EnableSsl = true, Credentials = new NetworkCredential(userName, password) };
        }

        static MailMessage PrepareMailMessage(string subject, string body)
        {
            return new MailMessage(userName, userName, subject, body);
        }
    }
}