using log4net;
using OpenQA.Selenium;

namespace Test_Automation_Frameworks.Pages
{
    public class MainPage : BasePage
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MainPage));
        private readonly By MagnifierIcon = By.XPath("//button[@class='header-search__button header__icon']");
        private readonly By SearchField = By.Name("q"); //Name locator
        private readonly By SearchFindButton = By.XPath("//span[normalize-space(text())='Find']/ancestor::button[contains(@class, 'custom-search-button')]"); //XPath locator with axes
        public MainPage(IWebDriver driver) : base(driver)
        {
            this.Url = "https://www.epam.com/?_gl=1*1lcowp0*_ga*ODU2ODQ2MzY0LjE3MjgxOTMxMTk.*_ga_WBGDS7S6W6*MTc0MTkwMjAxOS4xMS4xLjE3NDE5MDQwNTMuNjAuMC4w";
        }

        public SearchPage Search(string input)
        {
            Logger.Info($"[MAIN PAGE] Starting a search for '{input}'");
            ClickOnElement(MagnifierIcon);
            var searchField = GetClickableElement(SearchField);
            searchField.Click();
            searchField.SendKeys(input);
            GetClickableElement(SearchFindButton).Click();
            return new SearchPage(this.driver);
        }
    }
}