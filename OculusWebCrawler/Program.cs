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
using System.Net.Sockets;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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



                if (!File.Exists("../../../emails/errorLog_" + today + ".txt")) {
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

                    using (IWebDriver driver = new ChromeDriver())
                    {
                        driver.Navigate().GoToUrl("https://www.oculus.com/compare/?locale=pl_PL");

                        if (isQuestAvaliable(driver)){
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

                        if (isRiftSAvaliable(driver))
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
                        

                    
                    log("");
                    log("Iteration " + iteration + " completed... " + DateTime.Now);
                    iteration++;
                    log("Waiting 60 seconds...");
                    log("");
                    log("");
                    log("");
                    }
                }
                catch (Exception e) {
                    log(e.ToString());
                    log(DateTime.Now.ToString());
                }
                Thread.Sleep(60000);
            }
        }

        public static bool isQuestAvaliable(IWebDriver driver) {

            if(driver.FindElements(By.XPath("//*[@id=\"compare\"]/div[2]/section/div/div[2]/div/div[2]/div/div[2]/span")).Count == 0) return true;

            if (driver.FindElement(By.XPath("//*[@id=\"compare\"]/div[2]/section/div/div[2]/div/div[2]/div/div[2]/span")).Text == "Produkt niedostępny") return false;
            
            return true;
        }

        public static bool isRiftSAvaliable(IWebDriver driver) {

            if (driver.FindElements(By.XPath("//*[@id=\"compare\"]/div[2]/section/div/div[2]/div/div[3]/div/div[2]/span")).Count == 0) return true;

            if (driver.FindElement(By.XPath("//*[@id=\"compare\"]/div[2]/section/div/div[2]/div/div[3]/div/div[2]/span")).Text == "Produkt niedostępny") return false;

            return true;
        }

        public static void log(string text) {
            Console.WriteLine(text);
            File.AppendAllText("../../../emails/errorLog_" + today + ".txt", text + "\n");
        }

        
    }
}
