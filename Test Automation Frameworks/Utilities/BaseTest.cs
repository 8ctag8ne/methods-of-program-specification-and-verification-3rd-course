using log4net;
using log4net.Config;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace Test_Automation_Frameworks.Utilities
{
    public class BaseTest
    {
        protected WebPageDriver PageDriver;
        protected static readonly ILog Logger = LogManager.GetLogger(typeof(BaseTest));

        [OneTimeSetUp]
        public void GlobalSetUp()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            XmlConfigurator.Configure(new FileInfo(configPath));
        }

        [SetUp]
        public void SetUp()
        {
            Logger.Info($"[TEST STARTED] {TestContext.CurrentContext.Test.Name}");

            string browser = GetBrowserFromEnvironment();
            Logger.Info($"USING BROWSER: {browser}");
            PageDriver = new WebPageDriver(browser);
        }

        [TearDown]
        public void TearDown()
        {
            Logger.Info($"[TEST FINISHED] {TestContext.CurrentContext.Test.Name} - Status: {TestContext.CurrentContext.Result.Outcome.Status}");
            if(TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var path = ScreenshotHelper.TakeBrowserScreenshot((ITakesScreenshot)PageDriver.driver);
                Console.WriteLine(path);
            }
            SingletonWebDriver.Close();
        }

        protected string GetBrowserFromEnvironment()
        {
            
            // Перевіряємо змінні середовища
            var envBrowser = Environment.GetEnvironmentVariable("TestBrowser").ToLower();
            switch (envBrowser)
            {
                case "firefox":
                {
                    break;
                }
                case "edge":
                {
                    break;
                }
                default:
                {
                    envBrowser = "chrome";
                    break;
                }
            }
            if (!string.IsNullOrEmpty(envBrowser))
            {
                Logger.Info($"Found from environment: {envBrowser}");
                return envBrowser.ToLower();
            }
            
            Logger.Info("No environment parameters were found, using chrome");
            return "chrome";
        }
    }
}