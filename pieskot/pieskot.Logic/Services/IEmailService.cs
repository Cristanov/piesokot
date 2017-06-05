namespace NaSpacerDo.Logic.Services
{
    public interface IEmailService
    {
        void Send(string to, string subject, string body);
    }
}
