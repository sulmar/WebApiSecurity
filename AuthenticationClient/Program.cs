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
            Task.Run(() => BasicAuthenticationTest());

            Console.ReadKey();
        }

        private static async void BasicAuthenticationTest()
        {
            string username = "testuser";
            string password = "Pass1word";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:62986/");

                var request = new HttpRequestMessage(HttpMethod.Get, "api/values");

                var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                client.DefaultRequestHeaders.Authorization = authValue;

                var response = await client.SendAsync(request);

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
