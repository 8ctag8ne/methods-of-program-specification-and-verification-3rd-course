using OpenQA.Selenium;
using Test_Automation_Frameworks.Utilities;

namespace Test_Automation_Frameworks.Pages
{
    public class BasePage : WebPageDriver
    {
        public string Url { get;set; } = "https://www.epam.com/?_gl=1*1lcowp0*_ga*ODU2ODQ2MzY0LjE3MjgxOTMxMTk.*_ga_WBGDS7S6W6*MTc0MTkwMjAxOS4xMS4xLjE3NDE5MDQwNTMuNjAuMC4w";
        public BasePage(string browser) : base(browser){}
        public BasePage(IWebDriver driver) : base(driver){}
        private readonly By CareersTab = By.LinkText("Careers"); //LinkText locator
        private readonly By AboutTab = By.LinkText("About"); //LinkText locator
        private readonly By InsightsTab = By.LinkText("Insights"); //LinkText locator

        private readonly By AcceptCookiesButton = By.XPath("//button[@id='onetrust-accept-btn-handler' and text()='Accept All']"); //XPath locator with any operator
        
        public CareersPage GoToCarrersPage()
        {
            WaitUntilPageLoaded();
            this.ClickOnElement(CareersTab);
            return new CareersPage(this.driver);
        }
        public AboutPage GoToAboutPage()
        {
            WaitUntilPageLoaded();
            this.ClickOnElement(AboutTab);
            return new AboutPage(this.driver);
        }
        public InsightsPage GoToInsightsPage()
        {
            WaitUntilPageLoaded();
            this.ClickOnElement(InsightsTab);
            return new InsightsPage(this.driver);
        }

        public void AcceptCookies()
        {
            WaitUntilPageLoaded();
            Thread.Sleep(1000);
            ClickOnElement(AcceptCookiesButton);
        }

        public void RemoveJSDropdown()
        {
            ClickOnElement(By.TagName("body"));
        }

        public void Open()
        {
            this.driver.Navigate().GoToUrl(Url);
        }
    }
}