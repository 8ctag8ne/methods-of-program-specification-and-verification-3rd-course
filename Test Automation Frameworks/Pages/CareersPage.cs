using log4net;
using OpenQA.Selenium;

namespace Test_Automation_Frameworks.Pages
{
    public class CareersPage : BasePage
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CareersPage));
        private readonly By KeywordsField = By.Id("new_form_job_search-keyword"); //ID locator
        private readonly By LocationField = By.ClassName("recruiting-search__location"); //ClassName locator
        private readonly By AllLocationsOption = By.CssSelector("li.select2-results__option:not([aria-selected='true'])[title='All Locations']"); //CSS locator (if possible, use pseudo-classes)
        private readonly By RemoteToggle = By.ClassName("job-search__filter-items--remote"); //ClassName locator
        private readonly By PositionsFindButton = By.XPath("//button[@type='submit']"); //XPath locator (Relative path)
        private readonly By ViewAndApplyButton = By.XPath("//li[@class='search-result__item'][last()]//a[text()='View and apply']");
        
        private readonly By LastJob = By.XPath("//li[@class='search-result__item'][last()]");
        private readonly By JobTitle = By.TagName("h1"); //TagName locator
        public CareersPage(IWebDriver driver) : base(driver){}

        public void SelectLocation(By option)
        {
            ClickOnElement(LocationField);
            ClickOnElement(option);
        }
        public void SelectAllLocations()
        {
            Logger.Info("[CAREERS PAGE] Selecting 'All locations' option");
            SelectLocation(AllLocationsOption);
        }
        public void ToggleRemoteOption()
        {
            Logger.Info("[CAREERS PAGE] Toggling remote button");
            ClickOnElement(RemoteToggle);
        }

        public void FindPositions()
        {
            Logger.Info("[CAREERS PAGE] Clicking on 'Find' button");
            ClickOnElement(PositionsFindButton);
        }
        public void ViewLastJob()
        {
            Logger.Info("[CAREERS PAGE] Viewing last job on the page");
            ClickOnElement(LastJob);
            ClickOnElement(ViewAndApplyButton);
        }
        public string GetJobTitle()
        {
            Logger.Info("[CAREERS PAGE] Getting job title");
            return GetVisibleElement(JobTitle).Text;
        }

        public void EnterKeyword(string input)
        {
            Logger.Info($"[CAREERS PAGE] Entering keyword '{input}'");
            EnterInfo(KeywordsField, input);
            RemoveAutocompleteSuggestions();
        }

        public void RemoveAutocompleteSuggestions()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(@"
                document.querySelectorAll('.autocomplete-suggestions')
                    .forEach(el => el.remove());
            ");
        }
    }
}