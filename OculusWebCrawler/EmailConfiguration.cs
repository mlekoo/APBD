using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OculusWebCrawler
{
  

    public class EmailConfiguration
    {
        public string mailForQuestBody { get; set; }
        public string mailForRiftSBody { get; set; }
        public string mailForQuestTitle { get; set; }
        public string mailForRiftSTitle { get; set; }

        public EmailConfiguration(string titlePath, string bodyPath) {
            loadTitle(titlePath);
            loadBody(bodyPath);
        }

        

        private void loadBody(string bodyPath) {
            var lines = File.ReadLines(bodyPath);
            string device = "";
            StringBuilder stringBuilderForQuestBody = new StringBuilder();
            StringBuilder stringBuilderForRiftSBody = new StringBuilder();
            foreach (var line in lines) {
                if (line.Contains("quest:")) {
                    device = "quest";
                    continue;
                }
                if (line.Contains("rifts:")) {
                    device = "rifts";
                    continue;
                }

                if (device == "quest") {
                    stringBuilderForQuestBody.Append(line);
                    stringBuilderForQuestBody.Append("\n");
                }

                if (device == "rifts") {
                    stringBuilderForRiftSBody.Append(line);
                    stringBuilderForRiftSBody.Append("\n");

                }
            }

            this.mailForQuestBody = stringBuilderForQuestBody.ToString();
            this.mailForRiftSBody = stringBuilderForRiftSBody.ToString();


        }

        private void loadTitle(string titlePath) {
            var lines = File.ReadLines(titlePath);

            foreach (var line in lines) {
                if (line.Contains("quest:")) {
                    mailForQuestTitle = line.Replace("quest:", "");
                }

                if (line.Contains("rifts:")) {
                    mailForRiftSTitle = line.Replace("rifts:", "");
                }
            }

        }


    }
}
