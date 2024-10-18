using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;
using A11YAutomatedTests.Accessibility;

namespace A11YAutomatedTests;
public class AccessibilityApiClient
{
    private readonly RestClient _client;

    public AccessibilityApiClient()
    {
        var userName = Environment.GetEnvironmentVariable("LT_USERNAME", EnvironmentVariableTarget.Machine);
        var accessKey = Environment.GetEnvironmentVariable("LT_ACCESSKEY", EnvironmentVariableTarget.Machine);
        var options = new RestClientOptions()
        {
            BaseUrl = new Uri("https://api.lambdatest.com/accessibility/"),
            Authenticator = new HttpBasicAuthenticator(userName, accessKey),
            ThrowOnAnyError = true,
            FollowRedirects = true,
            MaxRedirects = 10,
            RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
        };

        _client = new RestClient(options);
        //_client.UseNewtonsoftJson();
    }

    public async Task<Root> GetTestIssueDataAsync(string testId, string impact = null, bool? bestPractice = null, bool? needsReview = null)
    {
        // Set up the request for the 'Get Test Issue Data' endpoint
        var request = new RestRequest("/api/v1/test-issue", Method.Get);

        // Add required query parameter
        request.AddQueryParameter("testId", testId);

        // Add optional query parameters if provided
        if (!string.IsNullOrEmpty(impact))
        {
            request.AddQueryParameter("impact", impact);
        }
        if (bestPractice.HasValue)
        {
            request.AddQueryParameter("bestPractice", bestPractice.ToString());
        }
        if (needsReview.HasValue)
        {
            request.AddQueryParameter("needsReview", needsReview.ToString());
        }

        var response = await _client.ExecuteAsync<Root>(request);
        return response.Data;
    }
}
