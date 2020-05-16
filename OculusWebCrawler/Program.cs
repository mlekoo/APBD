using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.IO;

namespace OculusWebCrawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine(Path.GetFullPath("../../../config/config.mycfg"));
            Console.WriteLine(Path.GetFullPath("../../../emails/emails.txt"));
            int iteration = 1;
            while (true)
            {
                Console.WriteLine(iteration + " iteration...");

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
                                if (emailContainer.mailsForQuest.Count >= 1)
                                {
                                    Console.WriteLine("Sending emails for Quest... ");
                                    Console.WriteLine("Emails count: " + emailContainer.mailsForQuest.Count);
                                    emailHandler.sendEmailsForQuest();
                                    Console.WriteLine("Emails sended...");
                                }
                                else
                                {
                                    Console.WriteLine("No emails for Quest awaiting...");
                                }
                            }

                            Console.WriteLine();

                            if (isRiftSAvaliable(htmlContent))
                            {
                                if (emailContainer.mailsForRiftS.Count >= 1)
                                {
                                    Console.WriteLine("Sending emails for RiftS... ");
                                    Console.WriteLine("Emails count: " + emailContainer.mailsForRiftS.Count);
                                    emailHandler.sendEmailsForRiftS();
                                    Console.WriteLine("Emails sended...");

                                }
                                else {
                                    Console.WriteLine("No emails for RiftS awaiting...");
                                }

                            }
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Iteration " + iteration + " completed...");
                iteration++;
                Console.WriteLine("Waiting 60 seconds...");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Thread.Sleep(60000);
            }
        }

        public static bool isQuestAvaliable(string htmlContent) {

            var regex = new Regex("\"Oculus Quest\",\"key\":\"questparent\",\"active\":false", RegexOptions.IgnoreCase);

            var matches = regex.Matches(htmlContent);

            foreach (var match in matches)
            {
                return true;
            }

            return false;
        }

        public static bool isRiftSAvaliable(string htmlContent) {

            var regex = new Regex("\"Oculus Rift S\",\"key\":\"rift-s-parent\",\"active\":false", RegexOptions.IgnoreCase);

            var matches = regex.Matches(htmlContent);

            foreach (var match in matches)
            {
                return true;
            }
            return false;
        }


        
    }
}
