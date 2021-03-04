# .NET example

The following .NET examples show how to create a GET and a POST signature in the correct form and send the request.
The code is tested on .NET 5.0/Windows

## Prereq's

- Install nuget package `jose-jwt`
- Generate private key with RSA key type

## The code
```csharp
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Jose;

class AuthenticationSample
{
    private static async Task MakeSamplePostRequest()
    {
        var requestUri = "https://api.mobeco.dk/some_specific_api/some_endpoint";
        var httpBody = System.Text.Json.JsonSerializer.Serialize(new {foo = "baz", bar = 5});
        var payload = requestUri + httpBody;

        var filePath = @"path_to_\your_private_key.pfx";
        var privateKeyCert = new X509Certificate2(filePath, "certificate_password");
        var bytes = Encoding.UTF8.GetBytes(payload);
        var hash = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(bytes));
        var privateKey = privateKeyCert.GetRSAPrivateKey();
        var signature = Jose.JWT.Encode(hash, privateKey, JwsAlgorithm.RS256);
        Debug.WriteLine($"Signature:\n{signature}");

        var request = new HttpRequestMessage()
        {
            Headers =
            {
                {"AuthenticationSignature", signature},
                {"Ocp-Apim-Subscription-Key", "your_mobilepay_api_subscription_key"}
            },
            Content = new StringContent(httpBody, Encoding.UTF8, "application/json"),
            Method = HttpMethod.Post,
            RequestUri = new Uri(requestUri)
        };

        using var httpClient = new HttpClient();
        var response = await httpClient.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();
        Debug.WriteLine($"\nResponse: {body}");
    }
    
    public async Task MakeSampleGetRequest()
    {
        var requestUri = "https://api.mobeco.dk/some_specific_api/some_endpoint?foo=bar";
        var payload = requestUri;

        var privateKeyCert = new X509Certificate2(@"path_to_\your_private_key.pfx");
        var bytes = Encoding.UTF8.GetBytes(payload);
        var hash = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(bytes));
        var privateKey = privateKeyCert.GetRSAPrivateKey();
        var signature = Jose.JWT.Encode(hash, privateKey, JwsAlgorithm.RS256);
        Debug.WriteLine($"Signature:\n{signature}");

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

        using var httpClient = new HttpClient();
        var response = await httpClient.SendAsync(request); 
        var body = await response.Content.ReadAsStringAsync();
        Debug.WriteLine($"\nResponse: {body}");
    }
}
```
