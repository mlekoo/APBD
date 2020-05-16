using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OculusWebCrawler
{
    public class Configuration
    {
        public string mailAddress { get; set; }
        public string password { get; set; }
        public string smtpClient { get; set; }
        public int port { get; set; }

        public Configuration(string path) {
            loadConfig(path);
        }

        private void loadConfig(string path) {
            var lines = File.ReadLines(path);

            foreach (var line in lines)
            {
                if (line.Contains("mailAddress:"))
                {
                    mailAddress = line.Replace("mailAddress:", "");
                }
                if (line.Contains("password:"))
                {
                    password = line.Replace("password:", "");
                }
                if (line.Contains("smtpClient:"))
                {
                    smtpClient = line.Replace("smtpClient:", "");
                }
                if (line.Contains("port:"))
                {
                    port = int.Parse(line.Replace("port:", ""));
                }
            }
        }

    }
}
