using NUnit.Framework;
using Test_Automation_Frameworks.Pages;
using Test_Automation_Frameworks.Utilities;

namespace Test_Automation_Frameworks.Tests;
[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class CareersPageTestFixture : BaseTest
{
    [TestCase(".NET")]
    [TestCase("Java")]
    [TestCase("Python")]
    [Category("UI")]
    public void LastJobOnCareersPageContainsInput(string input)
    {
        MainPage mainPage = new MainPage(PageDriver.driver);
        mainPage.Open();
        mainPage.AcceptCookies();
        var careersPage = mainPage.GoToCarrersPage();

        careersPage.EnterKeyword(input);
        careersPage.SelectAllLocations();
        careersPage.ToggleRemoteOption();
        careersPage.FindPositions();
        careersPage.ViewLastJob();

        var title = careersPage.GetJobTitle();
        Assert.That(title.Contains(input, StringComparison.CurrentCultureIgnoreCase));
    }
}