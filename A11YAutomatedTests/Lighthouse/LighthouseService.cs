using Newtonsoft.Json;
using A11YAutomatedTests.Performance;
using System.Linq.Expressions;
using System.Threading;

namespace A11YAutomatedTests;

public class LighthouseService
{
    private IWebDriver _driver;
    public ThreadLocal<Root> PerformanceReport { get; set; }

    public LighthouseService(IWebDriver driver)
    {
        _driver = driver;
        PerformanceReport = new ThreadLocal<Root>();
    }

    public void PerformLighthouseAnalysis()
    {
        var jsonResponse = new LambdaTestHooks(_driver).GenerateLighthouseReport();

        JsonSerializerSettings settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        PerformanceReport.Value = JsonConvert.DeserializeObject<Root>(jsonResponse["data"].ToString(), settings);
    }

    public void AssertFirstMeaningfulPaintScoreMoreThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Audits.FirstMeaningfulPaint.Score;
        PerformAssertion(actualValue > expected, $"{PerformanceReport.Value.Audits.FirstMeaningfulPaint.Title} should be > {expected} but was {actualValue}");
    }

    public void AssertFirstContentfulPaintScoreLessThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Audits.FirstContentfulPaint.Score;
        PerformAssertion(actualValue < expected, $"{PerformanceReport.Value.Audits.FirstContentfulPaint.Title} should be < {expected} but was {actualValue}");
    }

    public void AssertSpeedIndexScoreLessThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Audits.SpeedIndex.Score;
        PerformAssertion(actualValue < expected, $"{PerformanceReport.Value.Audits.SpeedIndex.Title} should be < {expected} but was {actualValue}");
    }

    public void AssertLargestContentfulPaintScoreLessThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Audits.LargestContentfulPaint.Score;
        PerformAssertion(actualValue < expected, $"{PerformanceReport.Value.Audits.LargestContentfulPaint.Title} should be < {expected} but was {actualValue}");
    }

    public void AssertInteractiveScoreLessThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Audits.Interactive.Score;
        PerformAssertion(actualValue < expected, $"{PerformanceReport.Value.Audits.Interactive.Title} should be < {expected} but was {actualValue}");
    }

    public void AssertTotalBlockingTimeLessThan(double expected)
    {
        double actualValue = double.Parse(PerformanceReport.Value.Audits.TotalBlockingTime.DisplayValue);
        PerformAssertion(actualValue < expected, $"{PerformanceReport.Value.Audits.TotalBlockingTime.Title} should be < {expected} but was {actualValue}");
    }

    public void AssertCumulativeLayoutShiftScoreLessThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Audits.CumulativeLayoutShift.Score;
        PerformAssertion(actualValue < expected, $"{PerformanceReport.Value.Audits.CumulativeLayoutShift.Title} should be < {expected} but was {actualValue}");
    }

    public void AssertRedirectScoreAboveThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Audits.Redirects.NumericValue;
        PerformAssertion(actualValue > expected, $"{PerformanceReport.Value.Audits.Redirects.Title} should be > {expected} but was {actualValue}");
    }

    public void AssertJavaExecutionTimeScoreAboveThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Audits.BootupTime.NumericValue;
        PerformAssertion(actualValue > expected, $"{PerformanceReport.Value.Audits.BootupTime.Title} should be > {expected} but was {actualValue}");
    }

    public void AssertSEOScoreAboveThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Categories.Seo.Score;
        PerformAssertion(actualValue > expected, $"{PerformanceReport.Value.Categories.Seo.Title} should be > {expected} but was {actualValue}");
    }

    public void AssertBestPracticesScoreAboveThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Categories.BestPractices.Score;
        PerformAssertion(actualValue > expected, $"{PerformanceReport.Value.Categories.BestPractices.Title} should be > {expected} but was {actualValue}");
    }

    public void AssertPWAScoreAboveThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Categories.Pwa.Score;
        PerformAssertion(actualValue > expected, $"{PerformanceReport.Value.Categories.Pwa.Title} should be > {expected} but was {actualValue}");
    }
    public void AssertPerformanceScoreAboveThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Categories.Performance.Score;
        PerformAssertion(actualValue > expected, $"{PerformanceReport.Value.Categories.Performance.Title} should be > {expected} but was {actualValue}");
    }

    // Accessibility Related Checks:

    // New Accessibility Assertion Methods
    public void AssertAccessibilityScoreAboveThan(double expected)
    {
        double actualValue = PerformanceReport.Value.Categories.Accessibility.Score;
        PerformAssertion(actualValue > expected, $"Accessibility score should be > {expected} but was {actualValue}");
    }

    public void AssertAriaAllowedAttributesValid()
    {
        double actualValue = PerformanceReport.Value.Audits.AriaAllowedAttr.Score;
        PerformAssertion(actualValue == 1, "ARIA allowed attributes are not valid.");
    }

    public void AssertAriaHiddenFocusValid()
    {
        double actualValue = (double)PerformanceReport.Value.Audits.AriaHiddenFocus.Score;
        PerformAssertion(actualValue == 1, "ARIA hidden focus validation failed.");
    }

    public void AssertColorContrastValid()
    {
        double actualValue = PerformanceReport.Value.Audits.ColorContrast.Score;
        PerformAssertion(actualValue == 1, "Color contrast does not meet accessibility standards.");
    }

    public void AssertImageAltValid()
    {
        double actualValue = PerformanceReport.Value.Audits.ImageAlt.Score;
        PerformAssertion(actualValue == 1, "Image alt attributes are missing or invalid.");
    }

    public void AssertLinkNameValid()
    {
        double actualValue = PerformanceReport.Value.Audits.LinkName.Score;
        PerformAssertion(actualValue == 1, "Some links are missing discernible names.");
    }

    public MetricPreciseValidationBuilder AssertMetric(Expression<Func<Root, object>> expression)
    {
        string metricName = TypePropertiesNameResolver.GetMemberName(expression);
        Func<Root, object> compiledExpression = expression.Compile();
        dynamic actualValue = compiledExpression(PerformanceReport.Value);

        return new MetricPreciseValidationBuilder(actualValue, metricName);
    }

    private static void PerformAssertion(bool condition, string message)
    {
        if (!condition)
        {
            throw new LighthouseAssertFailedException(message);
        }
    }
}
