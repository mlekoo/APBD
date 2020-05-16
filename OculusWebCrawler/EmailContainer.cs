using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OculusWebCrawler
{
    public class EmailContainer
    {
        public List<String> mailsForQuest { get; set; }
        public List<String> mailsForRiftS { get; set; }



        public EmailContainer(String path) {
            mailsForQuest = new List<String>();
            mailsForRiftS = new List<String>();

            loadEmails(path);
        }

        public void loadEmails(String path) {
            var lines = File.ReadLines(path);

            foreach (var line in lines) {
                var data = line.Split(";");

                if (validDate(data[2])) { // data[2] = valid_to_date


                    if (data[1].ToLower() == "quest") { // data[1] = quest or Rift S
                        if (validIfSended(data))
                        {
                            mailsForQuest.Add(data[0]);
                        }
                    }

                    if (data[1].ToLower() == "rifts")
                    {
                        if (validIfSended(data))
                        {
                            mailsForRiftS.Add(data[0]); // data[0] = emailAddress
                        }
                    }

                }
                
                
            }
        }

        public bool validDate(string date) {
            if (DateTime.Parse(date) >= DateTime.UtcNow)
            {
                return true;
            }

            return false;

        }

        public bool validIfSended(string[] userData) {
            var lines = File.ReadLines("../../../emails/usedEmails.txt");
            foreach (var line in lines) {

                var data = line.Split(";");
                
                if(userData[0] == data[0])
                {
                    if (userData[1] == data[1])
                    {
                        if ((DateTime.Today - DateTime.Parse(data[2])).TotalDays <= 3)
                        {
                            return false;
                        }
                    }

                }
            }

            return true;
            
        }

    }
}
