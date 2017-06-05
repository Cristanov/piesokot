using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NaSpacerDo.Logic.Services
{
    public class EmailService : IEmailService
    {
        public EmailService(string sender, string password, string host, int port)
        {
            Sender = sender;
            Password = password;
            Host = host;
            Port = port;
        }

        public string Sender { get; private set; }

        public string Password { get; private set; }

        public string Host { get; private set; }

        public int Port { get; private set; }

        public void Send(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage(Sender, to, subject, body);
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(Host, Port);
            var credential = new NetworkCredential()
            {
                UserName = Sender,
                Password = Password
            };
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
