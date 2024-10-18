using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;

namespace A11YAutomatedTests;

public class LighthouseApiClient
{
    private readonly RestClient _client;

    public LighthouseApiClient()
    {
        // Get username and access key from environment variables
        var userName = Environment.GetEnvironmentVariable("LT_USERNAME", EnvironmentVariableTarget.Machine);
        var accessKey = Environment.GetEnvironmentVariable("LT_ACCESSKEY", EnvironmentVariableTarget.Machine);

        // Configure RestClient options
        var options = new RestClientOptions()
        {
            BaseUrl = new Uri("https://api.lambdatest.com/lighthouse/"),
            Authenticator = new HttpBasicAuthenticator(userName, accessKey),
            ThrowOnAnyError = true,
            FollowRedirects = true,
            MaxRedirects = 10,
            RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
        };

        _client = new RestClient(options);
    }

    /// <summary>
    /// Fetch the Lighthouse performance report data by session ID.
    /// </summary>
    /// <param name="sessionId">SESSION ID</param>
    /// <returns>LighthouseReportResponse</returns>
    public async Task<RestResponse> GetLighthouseReportBySessionIdAsync(string sessionId)
    {
        if (string.IsNullOrEmpty(sessionId))
        {
            throw new ArgumentException("Session ID is required", nameof(sessionId));
        }

        // Create a RestRequest for the endpoint
        var request = new RestRequest($"/report/{sessionId}", Method.Get);

        // Execute the request asynchronously
        var response = await _client.ExecuteAsync(request);

        return response;
    }
}
