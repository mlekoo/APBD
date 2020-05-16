using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace OculusWebCrawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            while (true)
            {

                Configuration configuration = new Configuration("../../../config/config.mycfg");

                EmailContainer emailContainer = new EmailContainer("../../../emails/emails.txt");

                EmailConfiguration emailConfiguration = new EmailConfiguration("../../../emails/emailTitle.txt", "../../../emails/emailBody.txt");

                EmailHandler emailHandler = new EmailHandler(configuration, emailConfiguration, emailContainer);




                var url = "http://oculus.com/compare";

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var htmlContent = await response.Content.ReadAsStringAsync();

                            if (isQuestAvaliable(htmlContent))
                            {
                                emailHandler.sendEmailsForQuest();
                                Console.WriteLine("Sending emails for quest...");
                            }

                            if (isRiftSAvaliable(htmlContent))
                            {
                                emailHandler.sendEmailsForRiftS();
                                Console.WriteLine("Sending emails for rifts...");

                            }
                        }
                    }
                }
                Thread.Sleep(60000);
            }
        }

        public static bool isQuestAvaliable(string htmlContent) {

            var regex = new Regex("\"Oculus Quest\",\"key\":\"questparent\",\"active\":true", RegexOptions.IgnoreCase);

            var matches = regex.Matches(htmlContent);

            foreach (var match in matches)
            {
                return true;
            }

            return false;
        }

        public static bool isRiftSAvaliable(string htmlContent) {

            var regex = new Regex("\"Oculus Rift S\",\"key\":\"rift-s-parent\",\"active\":true", RegexOptions.IgnoreCase);

            var matches = regex.Matches(htmlContent);

            foreach (var match in matches)
            {
                return true;
            }
            return false;
        }


        
    }
}
