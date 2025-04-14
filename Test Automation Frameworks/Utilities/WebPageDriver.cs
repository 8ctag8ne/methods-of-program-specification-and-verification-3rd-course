using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Test_Automation_Frameworks.Utilities
{
    public class WebPageDriver
    {
        public readonly IWebDriver driver;
        public readonly WebDriverWait wait;
        public WebPageDriver(string browser, string downloadDirectory = "Downloads")
        {
            this.driver = SingletonWebDriver.GetDriver(browser);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
        }
        public WebPageDriver(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
        }

        public string GetCurrentUrl()
        {
            return driver.Url;
        }

        public void Close()
        {
            // Log.Info("Closing driver");
            SingletonWebDriver.Close();
        }

        public void GoBack()
        {
            // Log.Info("Returning to previous page.");
            driver.Navigate().Back();
        }
        public void GoToUrl(string Url)
        {
            driver.Navigate().GoToUrl(Url);
        }

        public void EnterInfo(By Field, string input)
        {
            var field = GetClickableElement(Field);
            field.Click();
            field.Clear();
            field.SendKeys(input);
        }

        public WebPageDriver WaitUntilPageLoaded()
        {
            // Log.Info("Waiting for page to load");
            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            return this;
        }

        public void ClickOnElement(By locator)
        {
            // Log.Info("Clicking on element by locator");
            WaitUntilPageLoaded()
            .GetClickableElement(locator)
            .Click();
        }
        public void ClickOnElement(IWebElement element)
        {
            // Log.Info("Clicking on WebElement");
            WaitUntilPageLoaded();
            element.Click();
        }

        public IWebElement GetElement(By locator)
        {
            // Log.Info("Getting element");
            return wait.Until(driver => driver.FindElement(locator));
        }
        public IWebElement GetClickableElement(By locator)
        {
            // Log.Info("Getting clickable element");
            return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }
        public IWebElement GetVisibleElement(By locator)
        {
            // Log.Info("Getting visible element");
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }
        public IList<IWebElement> GetElements(By locator)
        {
            // Log.Info("Getting multiple elements");
            WaitUntilPageLoaded();
            return wait.Until(driver => driver.FindElements(locator));
        }
    }
}