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
        public static string today = DateTime.Today.ToShortDateString().Replace("/", "-");
        public static async Task Main(string[] args)
        {
            log("Running... " + DateTime.Now);
            log("");
            int iteration = 1;
            while (true)
            {
                if (today != DateTime.Today.ToShortDateString().Replace("/", "-")) {
                    today = DateTime.Today.ToShortDateString().Replace("/", "-");
                }



                if (!File.Exists("../../../emails/errorLog_" + today + ".txt")){
                    File.WriteAllText("../../../emails/errorLog_" + today + ".txt", "");
                    if (iteration == 1) {
                        log(Path.GetFullPath("../../../config/config.mycfg"));
                        log(Path.GetFullPath("../../../emails/emails.txt"));
                    }
                }

                log(iteration + " iteration...");

                Configuration configuration = new Configuration("../../../config/config.mycfg");

                
                EmailContainer emailContainer = new EmailContainer("../../../emails/emails.txt");

                


                EmailConfiguration emailConfiguration = new EmailConfiguration("../../../emails/emailTitle.txt", "../../../emails/emailBody.txt");

                EmailHandler emailHandler = new EmailHandler(configuration, emailConfiguration, emailContainer);



                try
                {
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
                                        log("Sending emails for Quest... ");
                                        log("Emails count: " + emailContainer.mailsForQuest.Count);
                                        emailHandler.sendEmailsForQuest();
                                        log("Emails sended...");
                                    }
                                    else
                                    {
                                        log("No emails for Quest awaiting...");
                                    }
                                }
                                else
                                {
                                    log("Quest is unavaliable on oculus site...");
                                }

                                log("");

                                if (isRiftSAvaliable(htmlContent))
                                {
                                    if (emailContainer.mailsForRiftS.Count >= 1)
                                    {
                                        log("Sending emails for RiftS... ");
                                        log("Emails count: " + emailContainer.mailsForRiftS.Count);
                                        emailHandler.sendEmailsForRiftS();
                                        log("Emails sended...");

                                    }
                                    else
                                    {
                                        log("No emails for RiftS awaiting...");
                                    }

                                }
                                else
                                {
                                    log("RiftS is unavaliable on oculus site...");
                                }
                            }
                        }
                    }
                    log("");
                    log("Iteration " + iteration + " completed... " + DateTime.Now);
                    iteration++;
                    log("Waiting 60 seconds...");
                    log("");
                    log("");
                    log("");
                }
                catch (Exception e) {
                    log(e.ToString());
                    log(DateTime.Now.ToString());
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

        public static void log(string text) {
            Console.WriteLine(text);
            File.AppendAllText("../../../emails/errorLog_" + today + ".txt", text + "\n");
        }

        
    }
}
