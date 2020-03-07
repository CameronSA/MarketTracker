namespace MarketTracker.WebDriver
{
    using log4net;
    using MarketTracker.ViewModels;
    using MarketTracker.WebDriver.Objects;
    using MarketTracker.WebDriver.WebsiteProtocols.ShareCast;
    using MarketTracker.WebDriver.WebsiteProtocols.ShareCast.Procedures;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows;

    public class WebDriver
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static ChromeOptions chromeOptions;

        private static ChromeDriverService chromeDriverService;
        
        public static ApplicationViewModel ViewModel { get; private set; }

        public static bool Initialise(ApplicationViewModel viewModel)
        {
            if (viewModel.Model.DriverRunning || DriverRunning)
            {
                MessageBox.Show("Please wait for current process to finish before starting another one", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            viewModel.Model.DriverRunning = true;
            DriverRunning = true;
            ViewModel = viewModel;
            chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--incognito");
            chromeOptions.AddArgument("--headless");
            chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            Driver = new ChromeDriver(chromeDriverService, chromeOptions);
            return true;
        }

        public static bool DriverRunning { get; private set; }
        
        public static int FtseIndex { get; private set; }

        public static IWebDriver Driver { get; private set;}

        public static bool Setup(int ftseIndex)
        {
            FtseIndex = ftseIndex;
            var setup = new WebDriverSetup(ftseIndex);
            return setup.Successful;
        }

        public static void RunSetupProcedures(string websiteName)
        {
            try
            {
                GetAllCompaniesAndUrls(websiteName);
            }
            catch
            {
                MessageBox.Show("Unexpected Error", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        public static void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
            ViewModel.Model.DriverRunning = false;
            DriverRunning = false;
        }
        
        public static void GetAllCompaniesAndUrls(string websiteName)
        {
            string websiteUrl = WebDriverData.MarketSites[websiteName];
            bool foundWebsite = Navigate(websiteUrl);
            if (foundWebsite)
            {
                if (websiteName.ToLower() == "sharecast")
                {
                    var foundCompanies = ShareCast.GetCompaniesAndUrls();

                    if (!foundCompanies)
                    {
                        log.Error("Could not find any companies from website with url '" + websiteUrl + "'");
                        MessageBox.Show("Could not find any companies from website with url '" + websiteUrl + "'", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            else
            {
                log.Error("Could not find website with url '" + websiteUrl + "'");
                MessageBox.Show("Could not find website with url '" + websiteUrl + "'", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public static List<Company> GetCompanyNamesFromWebElements(WebElements data)
        {
            var companies = new List<Company>();
            int columnIndex = 0;
            var headerRows = data.headerElement.FindElements(By.TagName("tr"));
            var dataRows = data.dataElement.FindElements(By.TagName("tr"));
            if (headerRows.Count != 1)
            {
                log.Error(string.Format("Found wrong number of header rows. Expected 1, found {0}", headerRows.Count));
                return companies;
            }

            if (dataRows.Count < 1)
            {
                log.Error("Could not find data rows");
                return companies;
            }

            var headerColumns = headerRows[0].FindElements(By.TagName("th"));
            for (int i = 0; i < headerColumns.Count; i++)
            {
                var column = headerColumns[i];
                if (column.Text.Trim().ToLower() == "name")
                {
                    columnIndex = i;
                }
            }

            foreach (var dataRow in dataRows)
            {
                var company = new Company();
                var companyNameColumn = dataRow.FindElements(By.TagName("th"))[columnIndex];
                var companyNameAnchors = companyNameColumn.FindElements(By.TagName("a"));
                if (companyNameAnchors.Count != 1)
                {
                    log.Warn(string.Format("Found wrong number of anchor elements in column index '{0}'. Expected 1, found {1}. Row skipped.", columnIndex, companyNameAnchors.Count));
                    continue;
                }

                string companyName = companyNameAnchors[0].Text;
                string companyUrl = companyNameAnchors[0].GetAttribute("href");

                company.Name = companyName;
                company.Url = companyUrl;
                companies.Add(company);
                log.Info(string.Format("Found company name: '{0}', url: '{1}'", companyName, companyUrl));
            }

            return companies;
        }

        public static void WriteToFile(string filePath, List<Company> companies)
        {
            using (StreamWriter file = new StreamWriter(filePath))
            {
                foreach (var company in companies)
                {
                    file.WriteLine(company.Name + "," + company.Url);
                    log.Info(string.Format("Added company: '{0}' with url: '{1}' to file located at: '{2}'", company.Name, company.Url, filePath));
                }
            }
        }
                     
        public static bool Navigate(string websiteUrl)
        {
            Driver.Navigate().GoToUrl(websiteUrl);
            if ((Driver.Title.ToLower().Contains("page") && Driver.Title.ToLower().Contains("found"))
                    || Driver.Title.ToLower().Contains("webfg.com"))
            {
                log.Error(string.Format("Received an HTTP 404 error whilst attempting to access: '{0}' ({1})", websiteUrl, websiteUrl));
                return false;
            }

            log.Info(string.Format("Navigated to url: '{0}' ({1})", websiteUrl, websiteUrl));
            return true;
        }
    }
}
