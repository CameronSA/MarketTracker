namespace MarketTracker.WebDriver.WebsiteProtocols.ShareCast
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    using log4net;
    using MarketTracker.Data;
    using MarketTracker.WebDriver.Objects;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public static class ShareCast
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public static bool GetCompaniesAndUrls()
        {
            WebElements webElements = GetDataElements();
            var companies = new List<Company>();
            if (!webElements.tableFound)
            {
                log.Error("Could not find ShareCast data elements");
                MessageBox.Show("Could not find ShareCast data elements", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            companies = WebDriver.GetCompanyNamesFromWebElements(webElements);
            if (WebDriver.FtseIndex == 100)
            {
                WebDriverData.ShareCastCompanies100 = companies;
                WebDriver.WriteToFile(FilePaths.ShareCastCompanies100, companies);
                log.Info(string.Format("All company names and urls written to file located at '{0}'. Count: {1}", FilePaths.ShareCastCompanies100, companies.Count));
            }
            else if(WebDriver.FtseIndex == 250)
            {
                WebDriverData.ShareCastCompanies250 = companies;
                WebDriver.WriteToFile(FilePaths.ShareCastCompanies250, companies);
                log.Info(string.Format("All company names and urls written to file located at '{0}'. Count: {1}", FilePaths.ShareCastCompanies250, companies.Count));
            }

            return true;
        }

        private static WebElements GetDataElements()
        {
            var webElements = new WebElements();
            IWebElement headerElement = null;
            IWebElement dataElement = null;
            var wait = new WebDriverWait(WebDriver.Driver, TimeSpan.FromSeconds(10));
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
    }
}
