using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationClient
{
    class Program
    {
        static void Main(string[] args)
        {
            CertAuthenticationTest().Wait();

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

        private static X509Certificate GetClientCert()
        {
            X509Store store = null;
            try
            {
                store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

               // var certificateSerialNumber = "‎81 c6 62 0a 73 c7 b1 aa 41 06 a3 ce 62 83 ae 25".ToUpper().Replace(" ", string.Empty);

                //Does not work for some reason, could be culture related
                //var certs = store.Certificates.Find(X509FindType.FindBySerialNumber, certificateSerialNumber, true);

                //if (certs.Count == 1)
                //{
                //    var cert = certs[0];
                //    return cert;
                //}

                X509Certificate cert = store.Certificates[0];

                return cert;
            }
            finally
            {
                store?.Close();
            }
        }

        private static async Task CertAuthenticationTest()
        {
            string username = "testuser";
            string password = "Pass1word";

            X509Certificate certificate = X509Certificate.CreateFromCertFile(@"C:\Users\marci\Downloads\mycert.cer");

            // X509Certificate certificate = GetClientCert();

            WebRequestHandler handler = new WebRequestHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ClientCertificates.Add(certificate);
            handler.UseProxy = false;

            using (var client = new HttpClient(handler))
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                client.BaseAddress = new Uri("https://localhost:44365/");

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
