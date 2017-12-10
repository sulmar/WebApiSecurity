using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationClient
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicAuthenticationTest().Wait();

            Console.ReadKey();
        }


        private static async Task WindowsAuthenticationTest()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("http://localhost:49250/");

                var response = await client.GetAsync("api/values");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("{0}", result);
                }
                else
                {
                    Console.WriteLine("{0}", response.ReasonPhrase);
                }

            }
        }



        private static async Task BasicAuthenticationTest()
        {
            string username = "testuser";
            string password = "Pass1word";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62986/");

                var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                client.DefaultRequestHeaders.Authorization = authValue;

                var response = await client.GetAsync("api/values");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("{0}", result);
                }
                else
                {
                    Console.WriteLine("{0}", response.ReasonPhrase);
                }

            }
        }
    }
}
