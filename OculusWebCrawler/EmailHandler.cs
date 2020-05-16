using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace OculusWebCrawler
{
    public class EmailHandler
    {
        public Configuration configuration { get; set; }
        public EmailConfiguration emailConfiguration { get; set; }
        public EmailContainer emailContainer { get; set; }


        public EmailHandler(Configuration configuration, EmailConfiguration emailConfiguration, EmailContainer emailContainer) {
            this.configuration = configuration;
            this.emailConfiguration = emailConfiguration;
            this.emailContainer = emailContainer;
        }

        public void sendEmailsForQuest()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(configuration.smtpClient);
            mail.From = new MailAddress(configuration.mailAddress);

            StringBuilder sb = new StringBuilder();
            foreach (var email in emailContainer.mailsForQuest)
            {
                mail.To.Add(email);
                sb.Append(email + ";quest;" + DateTime.Today + "\n");
            }

            mail.Subject = emailConfiguration.mailForQuestTitle;
            mail.Body = emailConfiguration.mailForQuestBody;

            SmtpServer.Port = configuration.port;
            SmtpServer.Credentials = new System.Net.NetworkCredential(configuration.mailAddress, configuration.password);
            SmtpServer.EnableSsl = true;


            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            try
            {
                SmtpServer.Send(mail);
                File.AppendAllText("../../../emails/usedEmails.txt", sb.ToString());
            }
            catch (IOException e) {
                Console.WriteLine(e);
                
            }


        }

        public void sendEmailsForRiftS()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(configuration.smtpClient);
            mail.From = new MailAddress(configuration.mailAddress);

            StringBuilder sb = new StringBuilder();
            foreach (var email in emailContainer.mailsForRiftS)
            {
                mail.To.Add(email);
                sb.Append(email + ";rifts;" + DateTime.Today + "\n");
            }

            mail.Subject = emailConfiguration.mailForRiftSTitle;
            mail.Body = emailConfiguration.mailForRiftSBody;

            SmtpServer.Port = configuration.port;
            SmtpServer.Credentials = new System.Net.NetworkCredential(configuration.mailAddress, configuration.password);
            SmtpServer.EnableSsl = true;

            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            try
            {
                SmtpServer.Send(mail);
                File.AppendAllText("../../../emails/usedEmails.txt", sb.ToString());
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }

        }

    }
}
