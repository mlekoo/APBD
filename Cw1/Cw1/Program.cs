using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//email regex com

namespace Cw1
{
    class Program
    {
        public static async Task Main(string[] args)
        {
           // Console.WriteLine("Hello World!");
/*
            int? tmp1 = null;
            double tmp2 = 2.0;
            float d = 2f;
            string tmp3 = "dwosf";
            bool tmp4 = true;

            var tmp5 = "ala ma futrzaka";

            string tr = $"dwa {tmp1} {tmp2}";
            Person newPerson = new Person { FirstName = "Daniel" };*/

            var url = args.Length > 0 ? args[0] : "https://www.pja.edu.pl";

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    //string path = @"C:\Users\s19017\Desktop\Cw1";

                    if (response.IsSuccessStatusCode)
                    {
                        var htmlContent = await response.Content.ReadAsStringAsync();

                        var regex = new Regex("[a-z]+[a-z0-9]*@[a-z0-9]+//.[a-z]+", RegexOptions.IgnoreCase);
                        var matches = regex.Matches(htmlContent);

                        foreach (var match in matches)
                        {
                            Console.WriteLine(match.ToString());
                        }

                    }
                }
               

                
            }


           
        }
    }
}
