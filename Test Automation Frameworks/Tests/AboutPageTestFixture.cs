using NUnit.Framework;
using Test_Automation_Frameworks.Pages;
using Test_Automation_Frameworks.Utilities;

namespace Test_Automation_Frameworks.Tests;
[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class AboutPageTestFixture : BaseTest
{
    private string downloadDirectory;
    private FileDownloadHelper fileDownloadHelper;

    [SetUp]
    public void AdditionalSetUp()
    {
        downloadDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        fileDownloadHelper = new FileDownloadHelper();
    }

    [TestCase("EPAM_Corporate_Overview_Q4FY-2024.pdf")]
    [Category("UI")]
    public void CorrectFileIsDownloaded(string input)
    {
        MainPage mainPage = new MainPage(PageDriver.driver);
        mainPage.Open();
        mainPage.AcceptCookies();
        AboutPage aboutPage = mainPage.GoToAboutPage();
        fileDownloadHelper.DeleteFileIfExists(downloadDirectory, input);
        aboutPage.DownloadFile();
        fileDownloadHelper.VerifyFileDownload(downloadDirectory, input, TimeSpan.FromSeconds(60));
    }
}