using log4net;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Test_Automation_Frameworks.Pages
{
    public class SearchPage : MainPage
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SearchPage));
        private readonly By ArticlesList = By.XPath("//article[@class='search-results__item']//a[@class='search-results__title-link']");
        public SearchPage(IWebDriver driver) : base(driver){}

        public List<string> GetSearchResultsLinks(int count = 3)
        {
            Logger.Info("[SEARCH PAGE] Getting search result links");
            GetClickableElement(ArticlesList);
            return GetElements(ArticlesList).Select(a => a.GetAttribute("href")).Take(count).ToList();
        }

        public void ValidateThatLinksContainText(List<string> links, string inputText)
        {
            links.ForEach(article =>
            {
                Logger.Info($"[SEARCH PAGE] Validating {article}");
                GoToUrl(article);
                
                var text = GetElements(By.XPath("//ul[@class='scaling-of-text-wrapper']//li | //p"))
                                .Select(p => p.Text)
                                .Aggregate((a, b) => $"{a} {b}");

                Assert.That(text.Contains(inputText, StringComparison.CurrentCultureIgnoreCase), $"link: {article} does not contain {inputText}");

                GoBack();
            });
        }
    }
}