namespace MarketTracker.WebDriver
{
    using log4net;
    using MarketTracker.Data;
    using MarketTracker.WebDriver.Objects;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows;

    public class WebDriver : IDisposable
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ChromeOptions chromeOptions { get; }

        private ChromeDriverService chromeDriverService { get; }
        
        public WebDriver()
        {
            this.chromeOptions = new ChromeOptions();
            this.chromeOptions.AddArgument("--incognito");
            this.chromeDriverService = ChromeDriverService.CreateDefaultService();
            this.chromeDriverService.HideCommandPromptWindow = true;
            this.Driver = new ChromeDriver(this.chromeDriverService, this.chromeOptions);
        }

        public int ftseIndex { get; private set; }

        public IWebDriver Driver { get; }

        public bool Setup(int ftseIndex)
        {
            this.ftseIndex = ftseIndex;
            var setup = new WebDriverSetup(ftseIndex);
            return setup.Successful;
        }

        public void RunProcedures(string websiteName)
        {
            try
            {
                this.GetAllCompaniesAndUrls(websiteName);
            }
            catch
            {
                MessageBox.Show("Unexpected Error");
                return;
            }
        }
        
        public void Dispose()
        {
            this.Driver.Quit();
            this.Driver.Dispose();
        }
        
        private void GetAllCompaniesAndUrls(string websiteName) 
        {
            string websiteUrl = WebDriverData.MarketLinks[websiteName];
            bool foundWebsite = this.Navigate(websiteUrl);
            if(foundWebsite)
            {
                if(websiteName.ToLower() == "sharecast")
                {
                    var foundCompanies = this.GetShareCastCompaniesAndUrls();   
                    
                    if(!foundCompanies)
                    {
                        log.Error("Could not find any companies from website with url '" + websiteUrl + "'");
                        MessageBox.Show("Could not find any companies from website with url '" + websiteUrl + "'");
                        return;
                    }
                }
            }
            else
            {
                log.Error("Could not find website with url '" + websiteUrl + "'");
                MessageBox.Show("Could not find website with url '" + websiteUrl + "'");
                return;
            }
        }

        private bool GetShareCastCompaniesAndUrls()
        {
            WebElements webElements = this.GetShareCastDataElements();
            var companies = new List<Company>();
            if(!webElements.tableFound)
            {
                log.Error("Could not find ShareCast data elements");
                MessageBox.Show("Could not find ShareCast data elements");
                return false;
            }

            companies = this.GetCompanyNamesFromWebElements(webElements);
            this.WriteToFile(FilePaths.DataDirectory + "CompanyNames" + this.ftseIndex + ".dat", companies);
            log.Info(string.Format("All company names and urls written to file located at '{0}'. Count: {1}", FilePaths.DataDirectory + "CompanyNames" + this.ftseIndex + ".dat", companies.Count));
            return true;
        }

        private void WriteToFile(string filePath, List<Company> companies)
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

        private List<Company> GetCompanyNamesFromWebElements(WebElements data)
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

        private WebElements GetShareCastDataElements()
        {
            var webElements = new WebElements();
            IWebElement headerElement = null;
            IWebElement dataElement = null;
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(10));
            wait.PollingInterval = TimeSpan.FromSeconds(0.5);
            var tableElements = wait.Until(x => x.FindElements(By.TagName("table")));
            if (tableElements.Count != 1)
            {
                log.Error(string.Format("Found wrong number of instances of 'table' html element. Expected 1, found {0}", tableElements.Count.ToString()));
                webElements.tableFound = false;
                return webElements;
            }
            else
            {
                var theadElements = tableElements[0].FindElements(By.TagName("thead"));
                var tbodyElements = tableElements[0].FindElements(By.TagName("tbody"));
                headerElement = theadElements[0];
                dataElement = tbodyElements[0];

                if (theadElements.Count != 1)
                {
                    log.Error(string.Format("Found wrong number of instances of 'thead' html element. Expected 1, found {0}", tableElements.Count.ToString()));
                }

                if (tbodyElements.Count != 1)
                {
                    log.Error(string.Format("Found wrong number of instances of 'tbody' html element. Expected 1, found {0}", tableElements.Count.ToString()));
                }

                if (theadElements.Count != 1 || tbodyElements.Count != 1)
                {
                    webElements.tableFound = false;
                    return webElements;
                }
            }

            webElements.tableFound = true;
            webElements.headerElement = headerElement;
            webElements.dataElement = dataElement;
            return webElements;
        }

        private bool Navigate(string websiteUrl)
        {
            this.Driver.Navigate().GoToUrl(websiteUrl);
            if ((this.Driver.Title.ToLower().Contains("page") && this.Driver.Title.ToLower().Contains("found"))
                    || this.Driver.Title.ToLower().Contains("webfg.com"))
            {
                log.Error(string.Format("Received an HTTP 404 error whilst attempting to access: '{0}' ({1})", websiteUrl, websiteUrl));
                return false;
            }

            log.Info(string.Format("Navigated to url: '{0}' ({1})", websiteUrl, websiteUrl));
            return true;
        }

    }
}
