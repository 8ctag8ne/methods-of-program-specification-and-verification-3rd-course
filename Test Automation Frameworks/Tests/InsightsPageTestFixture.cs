using NUnit.Framework;
using Test_Automation_Frameworks.Pages;
using Test_Automation_Frameworks.Utilities;

namespace Test_Automation_Frameworks.Tests;
[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class InsightsPageTestFixture : BaseTest
{
    [Test]
    [Category("UI")]
    public void BannerTitleAndPageTitleAreTheSame()
    {
        MainPage mainPage = new MainPage(PageDriver.driver);
        mainPage.Open();
        mainPage.AcceptCookies();
        InsightsPage insightsPage = mainPage.GoToInsightsPage();
        insightsPage.ScrollNext(2);
        var bannerText = insightsPage.GetBannerText();
        insightsPage.ReadMore();

        var title = insightsPage.GetTitle();
        Assert.That(title.Trim().ToLower() == bannerText.Trim().ToLower(), $"Banner title was: '{bannerText}'; and page title was: '{title}'");
    }
}