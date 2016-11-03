//The following .NET examples show how to create a GET and a POST signature in the correct form and send the request.
//The nuget package "jose-jwt" was used to create a JWS.


    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using Jose;

    public class Class1
    {
        public async Task PostRequestExample()
        {
            var requestUri = "https://api.mobeco.dk/some_specific_api/some_endpoint";
            var httpBody = JsonConvert.SerializeObject(new { foo = "baz", bar = 5 });
            var payload = requestUri + httpBody;

            var privateKeyCert = new X509Certificate2(@"path_to_\your_private_key.pfx");
            var bytes = Encoding.UTF8.GetBytes(payload);
            var hash = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(bytes));
            var privateKey = privateKeyCert.PrivateKey as RSACryptoServiceProvider;
            var signature = JWT.Encode(hash, privateKey, JwsAlgorithm.RS256);

            var request = new HttpRequestMessage()
            {
                Headers = 
                { 
                    { "AuthenticationSignature", signature }, 
                    { "Ocp-Apim-Subscription-Key", "your_mobilepay_api_subscription_key" } 
                },
                Content = new StringContent(httpBody, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Post,
                RequestUri = new Uri(requestUri)
            };

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(request); 
            }
        }
        
        public async Task GetRequestExample()
        {
            var requestUri = "https://api.mobeco.dk/some_specific_api/some_endpoint?foo=bar";
            var payload = requestUri;

            var privateKeyCert = new X509Certificate2(@"path_to_\your_private_key.pfx");
            var bytes = Encoding.UTF8.GetBytes(payload);
            var hash = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(bytes));
            var privateKey = privateKeyCert.PrivateKey as RSACryptoServiceProvider;
            var signature = JWT.Encode(hash, privateKey, JwsAlgorithm.RS256);

            var request = new HttpRequestMessage()
            {
                Headers =
                {
                    { "AuthenticationSignature", signature },
                    { "Ocp-Apim-Subscription-Key", "your_mobilepay_api_subscription_key" }
                },
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri)
            };

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(request); 
            }
        }
    }
