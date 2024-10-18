using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;

namespace A11YAutomatedTests;
public class LambdaTestHooks
{
    private readonly IWebDriver _driver;
    private readonly IJavaScriptExecutor _jsExecutor;

    public LambdaTestHooks(IWebDriver driver)
    {
        _jsExecutor = (IJavaScriptExecutor)driver;
        _driver = driver;
    }

    // Check if a file exists on the test machine
    public void CheckFileExists(string fileName)
    {
        _jsExecutor.ExecuteScript($"lambda-file-exists={fileName}");
    }

    // Retrieve file metadata
    public void GetFileStats(string fileName)
    {
        _jsExecutor.ExecuteScript($"lambda-file-stats={fileName}");
    }

    // Download file content using base64 encoding
    public void GetFileContent(string fileName)
    {
        _jsExecutor.ExecuteScript($"lambda-file-content={fileName}");
    }

    // List files in the download directory
    public void ListFiles(string matchString)
    {
        _jsExecutor.ExecuteScript($"lambda-file-list={matchString}");
    }

    // Change the test name
    public void UpdateTestName(string testName)
    {
        _jsExecutor.ExecuteScript($"lambda-name={testName}");
    }

    // Update the build name
    public void UpdateBuildName(string buildName)
    {
        _jsExecutor.ExecuteScript($"lambda-build={buildName}");
    }

    // Mark test as passed/failed with reason
    public void SetActionStatus(string status, string reason)
    {
        var action = new { status, reason };
        _jsExecutor.ExecuteScript("lambda-action", action);
    }

    // Perform keyboard events
    public void PerformKeyboardEvent(string eventCommand)
    {
        _jsExecutor.ExecuteScript($"lambda-perform-keyboard-events:{eventCommand}");
    }

    // Abort test execution for live interaction
    public void Breakpoint()
    {
        _jsExecutor.ExecuteScript("lambda-breakpoint=true");
    }

    // Capture an asynchronous screenshot
    public void CaptureScreenshot()
    {
        _jsExecutor.ExecuteScript("lambda-screenshot=true");
    }

    // Delete files in the download directory
    public void DeleteFiles(params string[] files)
    {
        var filesList = string.Join(",", files);
        _jsExecutor.ExecuteScript($"lambda-files-delete={filesList}");
    }

    // Throttle network speed during test execution
    public void ThrottleNetwork(string profile)
    {
        _jsExecutor.ExecuteScript("lambda-throttle-network", profile);
    }

    // Fetch the IPs of the domain
    public void PingDomain(string domain)
    {
        _jsExecutor.ExecuteScript($"lambda-ping={domain}");
    }

    // Upload captured exceptions
    public void UploadExceptions(params string[] exceptions)
    {
        _jsExecutor.ExecuteScript("lambda-exceptions", exceptions);
    }

    // Print clipboard data
    public void GetClipboard()
    {
        _jsExecutor.ExecuteScript("lambda-get-clipboard");
    }

    // Set clipboard data
    public void SetClipboard(string data)
    {
        _jsExecutor.ExecuteScript($"lambda-set-clipboard={data}");
    }

    // Clear clipboard data
    public void ClearClipboard()
    {
        _jsExecutor.ExecuteScript("lambda-clear-clipboard");
    }

    // Fetch the IPs from the outbound domain
    public void UnboundPing(string domain)
    {
        _jsExecutor.ExecuteScript($"lambda-unbound-ping={domain}");
    }

    // Fetch network log entries
    public void FetchNetworkLog(string range = "all")
    {
        _jsExecutor.ExecuteScript($"lambda:network={range}");
    }

    // Update test name during test execution
    public void UpdateNameDuringExecution(string testName)
    {
        _jsExecutor.ExecuteScript($"lambdaUpdateName={testName}");
    }

    public Dictionary<string, object> GenerateLighthouseReport()
    {
        WebDriverWait wait = new WebDriverWait((IWebDriver)_driver, TimeSpan.FromMinutes(3));
        wait.IgnoreExceptionTypes(typeof(WebDriverException));

        return wait.Until(driver =>
        {
            try
            {
                var result = _jsExecutor.ExecuteScript($"lambdatest_executor: {{\"action\": \"generateLighthouseReport\", \"arguments\": {{\"url\": \"{_driver.Url}\"}}}}");

                if (result is Dictionary<string, object> report && !report.ContainsKey("error"))
                {
                    return report;
                }

                // Log or handle the "Failed to generate lighthouse report" message if needed.
                return null;
            }
            catch (WebDriverException)
            {
                // Returning null will cause WebDriverWait to continue trying until the timeout
                return null;
            }
        });
    }
}
