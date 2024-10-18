using Deque.AxeCore.Commons;
using Deque.AxeCore.Selenium;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;

namespace A11YAutomatedTests;

[TestFixture]
public class ProductPurchaseTests
{
    private IWebDriver _driver;
    private WebSite _webSite;
    private AxeBuilder _axeBuilder;

    [SetUp]
    public void TestInit()
    {
        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
        _driver = new ChromeDriver();
        _driver.Manage().Window.Maximize();

        _driver.Navigate().GoToUrl("https://ecommerce-playground.lambdatest.io/");
        _driver.Manage().Cookies.DeleteAllCookies();

        _webSite = new WebSite(_driver);
        _axeBuilder = new AxeBuilder(_driver);
    }

    [TearDown]
    public void TestCleanup()
    {
        _driver.Quit();
    }

    [Test]
    public void CorrectInformationDisplayedInCompareScreen_WhenOpenProductFromSearchResults_TwoProducts()
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

        AxeResult axeResult = _axeBuilder.Analyze(_webSite.HomePage.MainHeader);

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.CompareLastProduct();
        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct2.Id);
        _webSite.ProductPage.CompareLastProduct();

        _webSite.ProductPage.GoToComparePage();

        _webSite.ProductPage.AssertCompareProductDetails(expectedProduct1, 1);
        _webSite.ProductPage.AssertCompareProductDetails(expectedProduct2, 2);

        axeResult = _axeBuilder
            .WithRules("color-contrast", "duplicate-id")
            .WithTags("wcag2a")
            //.DisableRules("color-contrast")
            .Analyze();

        Assert.That(axeResult.Violations.Any(), Is.False);

        axeResult.Violations.Should().BeEmpty();

        // If you don't want to run Axe on iFrames you can tell Axe skip with AxeRunOptions.
        // Exclude may be combined with Include to scan a tree of elements but omit some children of that tree.
        //_axeBuilder
        //    .Include("#element-under-test")
        //    .Exclude("#element-under-test div.child-class-with-known-issues")
        //    .WithOptions(new AxeRunOptions()
        //    {
        //        Iframes = false
        //    })
        //    .Analyze();
    }

    [Test]
    public void PurchaseTwoSameProduct()
    {
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            Price = "$194.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg",
            Quantity = "2"
        };

        var userDetails = new UserDetails
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Telephone = "1234567890",
            Password = "password123",
            ConfirmPassword = "password123",
            AccountType = AccountOption.Register
        };

        var billingAddress = new BillingAddress
        {
            FirstName = "John",
            LastName = "Doe",
            Company = "Acme Corp",
            Address1 = "123 Main St",
            Address2 = "Apt 4",
            City = "Metropolis",
            PostCode = "12345",
            Country = "United States",
            Region = "Texas"
        };

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.AddToCart(expectedProduct1.Quantity);
        _webSite.CartPage.ViewCart();
        _webSite.CartPage.AssertTotalPrice("$388.00");

        _webSite.CartPage.Checkout();
        _webSite.CheckoutPage.FillUserDetails(userDetails);
        _webSite.CheckoutPage.FillBillingAddress(billingAddress);
        _webSite.CheckoutPage.AssertTotalPrice("$396.00");

        _webSite.CheckoutPage.AgreeToTerms();
        _webSite.CheckoutPage.CompleteCheckout();
    }
}
