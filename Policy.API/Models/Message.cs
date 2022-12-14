

using MailKit.Net.Smtp;
using MimeKit;
namespace Policy.API.Models
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x, null)));
            Subject = subject;
            Content = content;
        }
    }
}
