using ICSharpCode.SharpZipLib.Zip;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;

namespace A11YAutomatedTests;

/// <summary>
/// Represents a collection of functions to interact with the LambdaTest API endpoints.
/// </summary>
public class SessionApiClient
{
    private readonly RestClient _client;

    public SessionApiClient()
    {
        var userName = Environment.GetEnvironmentVariable("LT_USERNAME", EnvironmentVariableTarget.Machine);
        var accessKey = Environment.GetEnvironmentVariable("LT_ACCESSKEY", EnvironmentVariableTarget.Machine);
        var options = new RestClientOptions
        {
            BaseUrl = new Uri("https://api.lambdatest.com/automation"),
            Authenticator = new HttpBasicAuthenticator(userName, accessKey),
            ThrowOnAnyError = true,
            FollowRedirects = true,
            MaxRedirects = 10,
            RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
        };

        _client = new RestClient(options);
    }

    public async Task<RestResponse<Session>> GetSessionDetailsAsync(string sessionId, string shareExpiryLimit = "")
    {
        var request = new RestRequest($"/api/v1/sessions/{sessionId}", Method.Get);

        if (!string.IsNullOrEmpty(shareExpiryLimit))
        {
            request.AddQueryParameter("shareExpiryLimit", shareExpiryLimit);
        }

        return await _client.ExecuteAsync<Session>(request);
    }
}
