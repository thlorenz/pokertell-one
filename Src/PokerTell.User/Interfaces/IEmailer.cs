namespace PokerTell.User.Interfaces
{
    using System.IO;

    public interface IEmailer
    {
        void Send(string subject, string body);

        void Send(string subject, string body, FileInfo[] attachments);
    }
}