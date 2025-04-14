using OpenQA.Selenium;

namespace Test_Automation_Frameworks.Utilities
{
    public static class SingletonWebDriver
    {
        private static ThreadLocal<IWebDriver?> _webDriver = new ThreadLocal<IWebDriver?>();
        public static IWebDriver GetDriver(string browser = "chrome")
        {
            if(!_webDriver.IsValueCreated || _webDriver.Value == null)
            {
                _webDriver.Value = WebDriverFactory.CreateDriver(browser);
            }

            return _webDriver.Value;
        }

        public static void Close()
        {
            if(_webDriver.Value != null)
            {
                _webDriver.Value.Quit(); 
                _webDriver.Value.Dispose();
                _webDriver.Value = null;
            }
        }
    }
}