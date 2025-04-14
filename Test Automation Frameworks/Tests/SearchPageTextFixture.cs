using NUnit.Framework;
using Test_Automation_Frameworks.Pages;
using Test_Automation_Frameworks.Utilities;

namespace Test_Automation_Frameworks.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class SearchPageTextFixture : BaseTest
    {
        [TestCase("BLOCKCHAIN")]
        /* [TestCase("Cloud")]
        [TestCase("Automation")]*/
        [Category("UI")]
        public void AllSearchLinksContainInput(string input)
        {
            MainPage mainPage = new MainPage(PageDriver.driver);
            mainPage.Open();
            mainPage.AcceptCookies();
            SearchPage searchPage = mainPage.Search(input);
            var articlesLinks = searchPage.GetSearchResultsLinks();
            searchPage.ValidateThatLinksContainText(articlesLinks, input);
        }
    }
}