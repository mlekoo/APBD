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
                        mailsForQuest.Add(data[0]);
                    }

                    if (data[1].ToLower() == "rifts")
                    {
                        mailsForRiftS.Add(data[0]); // data[0] = emailAddress
                    }

                }
                
                
            }
        }

        public bool validDate(string date) {
            if (DateTime.Parse(date) <= DateTime.UtcNow)
            {
                return true;
            }

            return false;

        }

    }
}
