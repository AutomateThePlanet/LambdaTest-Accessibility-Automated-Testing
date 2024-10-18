using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using A11YAutomatedTests.Accessibility;

namespace A11YAutomatedTests;

[TestFixture]
public class ProductPurchaseTestsCloud
{
    private RemoteWebDriver _driver;
    private WebSite _webSite;
    private LighthouseService _lighthouseService;

    [SetUp]
    public void TestInit()
    {
        string userName = Environment.GetEnvironmentVariable("LT_USERNAME", EnvironmentVariableTarget.Machine);
        string accessKey = Environment.GetEnvironmentVariable("LT_ACCESSKEY", EnvironmentVariableTarget.Machine);
        ChromeOptions options = new ChromeOptions();

        options.AddAdditionalOption("user", userName);
        options.AddAdditionalOption("accessKey", accessKey);

        var buildName = TimestampBuilder.BuildUniqueText("PO_");


        var capabilities = new Dictionary<string, object>
        {
            { "resolution", "1920x800" },
            { "platform", "Windows 10" },
            { "visual", "false" },
            { "video", "true" },
            { "seCdp", "true" },
            { "console", "true" },
            { "w3c", "true" },
            { "plugin", "c#-c#" },
            { "build", buildName },
            //{ "performance", "true" },
            { "project", "A11Y_RUN" },
            { "selenium_version", "4.22.0" },
            //{ "idleTimeout", "300" }
            { "accessibility", "true" }, // Enable accessibility testing
            { "accessibility.wcagVersion", "wcag21a" }, // Specify WCAG version (e.g., WCAG 2.1 Level A)
            { "accessibility.bestPractice", "true" }, // Exclude best practice issues from results
            { "accessibility.needsReview", "true" },  // Include issues that need review
        };

        _driver = new RemoteWebDriver(new Uri($"https://{userName}:{accessKey}@hub.lambdatest.com/wd/hub"), options);
        _driver.Manage().Window.Maximize();

        _driver.Navigate().GoToUrl("https://ecommerce-playground.lambdatest.io/");
        _driver.Manage().Cookies.DeleteAllCookies();

        _webSite = new WebSite(_driver);
    }

    [TearDown]
    public void TestCleanup()
    {
        _driver.Quit();
    }

    [Test]
    public void CorrectInformationDisplayedInCompareScreen_CheckA11Y_LambdaTestA11YAutomation()
    {
        // Arrange
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            Price = "$194.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg"
        };

        var expectedProduct2 = new ProductDetails
        {
            Name = "iPod Shuffle",
            Id = 34,
            Price = "$182.00",
            Model = "Product 7",
            Brand = "Apple",
            Weight = "5.00kg"
        };

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.CompareLastProduct();
        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct2.Id);
        _webSite.ProductPage.CompareLastProduct();

        _webSite.ProductPage.GoToComparePage();

        //_webSite.ProductPage.AssertCompareProductDetails(expectedProduct1, 1);
        //_webSite.ProductPage.AssertCompareProductDetails(expectedProduct2, 2);

        // call SDK to get results from LT Accessibility - if there any failures
        // fail test with message and point to the portal.
        var sessionApiClient = new SessionApiClient();
        var accessibilityApiClient = new AccessibilityApiClient();
        var currentSession = sessionApiClient.GetSessionDetailsAsync(_driver.SessionId.ToString()).Result;
        var accessibilityResults = accessibilityApiClient.GetTestIssueDataAsync(currentSession.Data.Data.TestId).Result;

        Assert.That(accessibilityResults.TestInfo.TotalIssues, Is.EqualTo(0), $"{accessibilityResults.ScanJson.First().IssueSummary} /n Check LT A11Y Automation Dashboard for more details!");
    }

    [Test]
    public void CorrectInformationDisplayedInCompareScreen_CheckA11Y_GoogleLighthouse()
    {
        // Arrange
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            Price = "$194.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg"
        };

        var expectedProduct2 = new ProductDetails
        {
            Name = "iPod Shuffle",
            Id = 34,
            Price = "$182.00",
            Model = "Product 7",
            Brand = "Apple",
            Weight = "5.00kg"
        };

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.CompareLastProduct();
        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct2.Id);
        _webSite.ProductPage.CompareLastProduct();

        _webSite.ProductPage.GoToComparePage();

        _lighthouseService.PerformLighthouseAnalysis();

        _lighthouseService.AssertAccessibilityScoreAboveThan(0.4);
        _lighthouseService.AssertAriaAllowedAttributesValid();
        _lighthouseService.AssertAriaHiddenFocusValid();
        _lighthouseService.AssertColorContrastValid();
        _lighthouseService.AssertImageAltValid();
        _lighthouseService.AssertLinkNameValid();
        _lighthouseService.AssertMetric(_ => _lighthouseService.PerformanceReport.Value.Audits.ColorContrast.Score > 0.2);
    }
}
