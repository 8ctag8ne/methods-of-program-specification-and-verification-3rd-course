using OpenQA.Selenium;

namespace Test_Automation_Frameworks.Utilities
{
    public static class ScreenshotHelper
    {
        public static string TakeBrowserScreenshot(ITakesScreenshot driver)
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-fff");
            var folder = "Screenshots";
            Directory.CreateDirectory(folder); 
            var rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
            Directory.CreateDirectory(rootPath);
            var screenshotPath = Path.Combine(rootPath, $"Display_{now}.png");
            driver.GetScreenshot().SaveAsFile(screenshotPath);
    
            return screenshotPath;
        }
    }
}