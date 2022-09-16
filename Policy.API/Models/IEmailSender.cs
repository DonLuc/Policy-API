namespace Policy.API.Models
{
    public interface IEmailSender
    {
        void sendEmail(Message message);
    }
}
