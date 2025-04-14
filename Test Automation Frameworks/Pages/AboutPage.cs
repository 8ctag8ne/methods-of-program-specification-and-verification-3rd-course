using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Test_Automation_Frameworks.Pages
{
    public class AboutPage : MainPage
    {
        private By DownloadButton = By.XPath("//span[contains(@class, 'button__content--desktop') and normalize-space()='DOWNLOAD']");
        public AboutPage(IWebDriver driver) : base(driver){}

        private static readonly ILog Logger = LogManager.GetLogger(typeof(AboutPage));
        public void DownloadFile()
        {
            Logger.Info("[ABOUT PAGE] Trying to download file...");
            Actions actions = new Actions(driver);
            var button = GetElement(DownloadButton);
            actions.MoveToElement(button);
            actions.Click(button);
            actions.Perform();
        }
    }
}