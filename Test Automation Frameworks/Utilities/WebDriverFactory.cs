using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace Test_Automation_Frameworks.Utilities;
public static class WebDriverFactory
{
    public static IWebDriver CreateDriver(string browser, string downloadDirectory = "Downloads")
    {
        // Отримати шлях до директорії для завантажень
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), downloadDirectory);
        
        switch(browser.ToLower())
        {
            case "chrome":
            {
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                options.AddArgument("--disable-extensions");
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-gpu");

                options.AddUserProfilePreference("download.default_directory", directory);
                options.AddUserProfilePreference("download.prompt_for_download", false);
                options.AddUserProfilePreference("download.directory_upgrade", true);
                options.AddUserProfilePreference("safebrowsing.enabled", true);

                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                return new ChromeDriver(service, options, TimeSpan.FromSeconds(120));
            }
            case "firefox":
            {
                var options = new FirefoxOptions();
                options.AddArgument("--start-maximized");
                options.AddArgument("--disable-extensions");
                
                // Firefox використовує інші налаштування для завантажень
                var firefoxProfile = new FirefoxProfile();
                firefoxProfile.SetPreference("browser.download.folderList", 2);
                firefoxProfile.SetPreference("browser.download.dir", directory);
                firefoxProfile.SetPreference("browser.download.useDownloadDir", true);
                firefoxProfile.SetPreference("browser.download.viewableInternally.enabledTypes", "");
                firefoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf;text/plain;application/text;text/xml;application/xml");
                firefoxProfile.SetPreference("pdfjs.disabled", true);  // Вимкнути вбудований PDF viewer
                
                options.Profile = firefoxProfile;
                return new FirefoxDriver(options);
            }
            case "edge":
            {
                var options = new EdgeOptions();
                options.AddArgument("--start-maximized");
                options.AddArgument("--disable-extensions");
                
                // Edge базується на Chromium, тому використовує ті ж самі налаштування, що й Chrome
                options.AddUserProfilePreference("download.default_directory", directory);
                options.AddUserProfilePreference("download.prompt_for_download", false);
                options.AddUserProfilePreference("download.directory_upgrade", true);
                options.AddUserProfilePreference("safebrowsing.enabled", true);
                return new EdgeDriver(options);
            }
            default:
            {
                throw new NotSupportedException($"{browser} browser is not supported");
            }
        }
    }
}