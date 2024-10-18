namespace A11YAutomatedTests;
public class HomePage : WebPage
{
    public HomePage(IWebDriver driver, WebDriverWait wait, Actions actions) 
        : base(driver, wait, actions)
    {
    }

    public IWebElement SearchInput => _driver.FindElement(By.XPath("//input[@name='search']"));
    public IWebElement MainHeader => _driver.FindElement(By.Id("main-header"));

    public void SearchProduct(string searchText)
    {
        SearchInput.Clear();
        SearchInput.SendKeys(searchText);
        //SearchInput.SendKeys(Keys.Enter);
    }
}
