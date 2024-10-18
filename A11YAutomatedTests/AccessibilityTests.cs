using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using WebDriverManager.DriverConfigs.Impl;

namespace A11YAutomatedTests;
[TestFixture]
public class AccessibilityTests
{
    private IWebDriver _driver;

    [SetUp]
    public void TestInit()
    {
        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();
        _driver.Navigate().GoToUrl("https://ecommerce-playground.lambdatest.io/");
        _driver.Manage().Cookies.DeleteAllCookies();
    }

    [TearDown]
    public void TestCleanup()
    {
        _driver.Quit();
    }

    [Test]
    public void VerifyNoDuplicateIds()
    {
        // Check for duplicate ID attributes on the page
        var allElements = _driver.FindElements(By.XPath("//*[@id]"));
        var ids = allElements.Select(e => e.GetAttribute("id"));
        var duplicateIds = ids.GroupBy(id => id).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

        duplicateIds.Should().BeEmpty("there should be no duplicate IDs on the page");
    }

    [Test]
    public void VerifyColorContrastIssues()
    {
        // Manually check if the text and background colors have sufficient contrast
        var textElement = _driver.FindElement(By.CssSelector("body .some-text-class"));
        var backgroundColor = textElement.GetCssValue("background-color");
        var textColor = textElement.GetCssValue("color");

        // Convert colors from RGB to hex
        var bgColorHex = ConvertRgbToHex(backgroundColor);
        var textColorHex = ConvertRgbToHex(textColor);

        // Use a formula to manually calculate contrast ratio (simplified for this example)
        var contrastRatio = CalculateContrastRatio(bgColorHex, textColorHex);

        // Assert contrast ratio (typically a value >= 4.5:1 for small text)
        Assert.That(contrastRatio, Is.GreaterThanOrEqualTo(4.5), "Text should have sufficient contrast with the background.");
    }

    [Test]
    public void VerifyImageHasAltAttribute()
    {
        // Check if images have alt attributes
        var images = _driver.FindElements(By.TagName("img"));
        foreach (var img in images)
        {
            var altText = img.GetAttribute("alt");
            altText.Should().NotBeNullOrEmpty($"Image with src '{img.GetAttribute("src")}' should have an alt attribute.");
        }
    }

    [Test]
    public void VerifyLinksHaveDiscernibleNames()
    {
        // Check if links have discernible text or aria-label attributes
        var links = _driver.FindElements(By.TagName("a"));
        foreach (var link in links)
        {
            var linkText = link.Text;
            var ariaLabel = link.GetAttribute("aria-label");

            // Ensure the link has either text or an aria-label
            bool hasDiscernibleName = !string.IsNullOrEmpty(linkText) || !string.IsNullOrEmpty(ariaLabel);

            hasDiscernibleName.Should().BeTrue($"Link with href '{link.GetAttribute("href")}' should have discernible text or aria-label.");
        }
    }

    // Check if elements are focusable using the 'Tab' key
    [Test]
    public void VerifyKeyboardNavigation()
    {
        // Attempt to focus on elements that should be navigable via keyboard (e.g., buttons, links, inputs)
        var focusableElements = _driver.FindElements(By.CssSelector("a, button, input, select, textarea, [tabindex='0']"));

        foreach (var element in focusableElements)
        {
            bool isFocusable = IsElementFocusable(element);
            Assert.That(isFocusable, Is.True, $"Element '{element.TagName}' with text '{element.Text}' should be keyboard-focusable.");
        }
    }

    [Test]
    public void VerifyTabOrderAndKeyboardNavigation()
    {
        // Get all focusable elements in the order they should be navigated by Tab
        var focusableElements = _driver.FindElements(By.CssSelector("a, button, input, select, textarea, [tabindex], [contenteditable='true']"));

        // Start focusing on the first focusable element
        focusableElements[0].SendKeys(Keys.Tab);

        // Verify tab navigation moves in the correct order
        for (int i = 0; i < focusableElements.Count - 1; i++)
        {
            // Move focus to the next element
            focusableElements[i].SendKeys(Keys.Tab);

            // Verify that the active element matches the expected next element in the tab order
            var activeElement = _driver.SwitchTo().ActiveElement();
            var nextElement = focusableElements[i + 1];

            Assert.That(activeElement, Is.EqualTo(nextElement),
                $"Tab order is incorrect. Expected focus on '{nextElement.TagName}' but found it on '{activeElement.TagName}'.");
        }
    }

    // Check if interactive elements (buttons, links) are operable via keyboard (Enter or Space key)
    [Test]
    public void VerifyKeyboardOperability()
    {
        var interactiveElements = _driver.FindElements(By.CssSelector("a, button, [role='button'], input[type='button'], input[type='submit']"));

        foreach (var element in interactiveElements)
        {
            bool isOperable = IsElementOperableByKeyboard(element);
            Assert.That(isOperable, Is.True, $"Interactive element '{element.TagName}' with text '{element.Text}' should be operable by keyboard.");
        }
    }

    // Check if elements have sufficient touch target size (minimum size recommendation: 44x44 pixels)
    [Test]
    public void VerifyTouchTargetSize()
    {
        var touchElements = _driver.FindElements(By.CssSelector("a, button, input[type='button'], input[type='submit']"));

        foreach (var element in touchElements)
        {
            var width = element.Size.Width;
            var height = element.Size.Height;

            // Assert touch target size is at least 44x44 pixels
            Assert.That(width >= 44 && height >= 44, Is.True, $"Touch target (e.g., button) '{element.TagName}' with text '{element.Text}' is too small (Size: {width}x{height}). Minimum recommended size is 44x44 pixels.");
        }
    }

    // Check if form elements have associated labels
    [Test]
    public void VerifyFormLabels()
    {
        var formControls = _driver.FindElements(By.CssSelector("input, select, textarea"));

        foreach (var control in formControls)
        {
            var label = _driver.FindElements(By.CssSelector($"label[for='{control.GetAttribute("id")}']"));
            Assert.That(label.Count > 0, Is.True, $"Form control '{control.TagName}' with ID '{control.GetAttribute("id")}' does not have an associated label.");
        }
    }

    // Check if heading elements are in sequential order (e.g., no skipping from H2 to H4)
    [Test]
    public void VerifyHeadingOrder()
    {
        var headings = _driver.FindElements(By.CssSelector("h1, h2, h3, h4, h5, h6"));
        var headingLevels = headings.Select(h => int.Parse(h.TagName.Replace("h", ""))).ToList();

        for (int i = 0; i < headingLevels.Count - 1; i++)
        {
            Assert.That(headingLevels[i] <= headingLevels[i + 1], Is.True, $"Heading '{headings[i].TagName}' should not skip levels. Found '{headings[i + 1].TagName}' immediately after.");
        }
    }

        // Helper method to check if an element can receive keyboard focus
    private bool IsElementFocusable(IWebElement element)
    {
        // Try sending the Tab key to focus on the element
        try
        {
            _driver.ExecuteJavaScript("arguments[0].focus();", element);
            return element.Equals(_driver.SwitchTo().ActiveElement());
        }
        catch
        {
            return false;
        }
    }

    // Helper method to simulate key press (Enter or Space) to check if the element is operable
    private bool IsElementOperableByKeyboard(IWebElement element)
    {
        try
        {
            // Send Enter key to the element
            element.SendKeys(Keys.Enter);
            return true; // If no exception occurs, it means the element is operable
        }
        catch
        {
            return false; // If an exception occurs, the element may not be operable
        }
    }

    // Helper function to convert CSS RGB color to hex
    private string ConvertRgbToHex(string rgb)
    {
        // Example: "rgb(255, 255, 255)" -> "#FFFFFF"
        var regex = new System.Text.RegularExpressions.Regex(@"rgba?\((\d+), (\d+), (\d+)");
        var match = regex.Match(rgb);
        if (!match.Success) return null;

        var r = int.Parse(match.Groups[1].Value);
        var g = int.Parse(match.Groups[2].Value);
        var b = int.Parse(match.Groups[3].Value);

        return $"#{r:X2}{g:X2}{b:X2}";
    }

    // Helper function to calculate contrast ratio (simplified example)
    private double CalculateContrastRatio(string bgColorHex, string textColorHex)
    {
        // Simplified example using luminance formula, normally this would require a more accurate calculation
        var bgLuminance = CalculateLuminance(bgColorHex);
        var textLuminance = CalculateLuminance(textColorHex);

        return (Math.Max(bgLuminance, textLuminance) + 0.05) / (Math.Min(bgLuminance, textLuminance) + 0.05);
    }

    private double CalculateLuminance(string colorHex)
    {
        // Convert hex color to RGB values
        int r = Convert.ToInt32(colorHex.Substring(1, 2), 16);
        int g = Convert.ToInt32(colorHex.Substring(3, 2), 16);
        int b = Convert.ToInt32(colorHex.Substring(5, 2), 16);

        // Convert RGB to relative luminance
        var rL = (r / 255.0 <= 0.03928) ? r / 255.0 / 12.92 : Math.Pow((r / 255.0 + 0.055) / 1.055, 2.4);
        var gL = (g / 255.0 <= 0.03928) ? g / 255.0 / 12.92 : Math.Pow((g / 255.0 + 0.055) / 1.055, 2.4);
        var bL = (b / 255.0 <= 0.03928) ? b / 255.0 / 12.92 : Math.Pow((b / 255.0 + 0.055) / 1.055, 2.4);

        // Return relative luminance
        return 0.2126 * rL + 0.7152 * gL + 0.0722 * bL;
    }
}
