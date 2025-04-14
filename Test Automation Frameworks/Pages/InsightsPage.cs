using log4net;
using OpenQA.Selenium;

namespace Test_Automation_Frameworks.Pages
{
    public class InsightsPage : BasePage
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InsightsPage));
        private By ImageBanner = By.XPath("//img[@class='single-slide__image']");
        private By RightButton = By.XPath("//button[contains(@class,'slider__right-arrow')]");
        private By ActiveElementLocator = By.XPath("//div[contains(@class, 'active')][contains(@class, 'owl-item')][contains(@aria-hidden, 'false')]");
        private IWebElement? ActiveElement = null;
        private By ReadMoreButton =  By.XPath(".//a[contains(text(), 'Read More')]");
        private By Title =  By.XPath("//span[@class='font-size-80-33']//span[@class='museo-sans-light']");

        private By BannerTextLocator = By.XPath(".//div[@class='text-ui-23']");
        public InsightsPage(IWebDriver driver) : base(driver){}

        public void ScrollNext(int times = 1)
        {
            Logger.Info("[INSIGHTS PAGE] Scrolling to next banner");
            for(int i = 0; i < times; i++)
            {
                ClickOnElement(RightButton);
                Thread.Sleep(1000);
            }
        }
        public IWebElement GetActiveElement()
        {
            if(ActiveElement == null)
            {
                ActiveElement = GetElements(ActiveElementLocator)[0];
            }
            return ActiveElement;
        }

        public string GetBannerText()
        {
            Logger.Info("[INSIGHTS PAGE] Getting banner text");
            var element = GetActiveElement();
            string fullText = wait.Until(driver => element.FindElement(BannerTextLocator)).GetAttribute("textContent").Replace("\r\n", "").Trim();
            Logger.Info($"[INSIGHTS PAGE] Retrieved banner text: {fullText}");

            return fullText;
        }

        public void ReadMore()
        {
            Logger.Info("[INSIGHTS PAGE] Clicking on 'Read more' button");
            var activeElement = GetActiveElement();
            var readMoreButton = activeElement.FindElement(ReadMoreButton);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(readMoreButton));
            readMoreButton.Click();
        }

        public string GetTitle()
        {
            return GetElement(Title).Text.Trim();
        }
    }
}