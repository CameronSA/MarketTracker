namespace MarketTracker.WebDriver.WebsiteProtocols.ShareCast.Procedures
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using log4net;
    using MarketTracker.Data;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public static class ShareCastSharePrices
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Download(bool allCompanies, bool allRecords, string companyName = null, DateTime startDate = new DateTime(), DateTime endDate = new DateTime())
        {
            string companyUrl = WebDriver.FindCompany(companyName);
            if(string.IsNullOrEmpty(companyUrl) && !allCompanies)
            {
                MessageBox.Show("Please select a company", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string downloadUrl = companyUrl + "/share-prices/download";
            WebDriver.Navigate(downloadUrl);
            string fileStartDate = startDate.Date.ToString().Replace(".", string.Empty).Replace(":", string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty);
            string fileEndDate = endDate.Date.ToString().Replace(".", string.Empty).Replace(":", string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty);
            SelectDateRange(startDate, endDate, allRecords, companyName + "_" + fileStartDate + "_" + fileEndDate + ".dat");
        }

        private static void SelectDateRange(DateTime startDate, DateTime endDate, bool allRecords, string fileName)
        {
            string currentUrl = WebDriver.Driver.Url;            
            var wait = new WebDriverWait(WebDriver.Driver, TimeSpan.FromSeconds(10))
            {
                PollingInterval = TimeSpan.FromSeconds(0.5)
            };

            IWebElement startDay = null;
            IWebElement startMonth = null;
            IWebElement startYear = null;
            IWebElement endDay = null;
            IWebElement endMonth = null;
            IWebElement endYear = null;
            IWebElement downloadFormat = null;
            
            try
            {
                startDay = wait.Until(x => x.FindElement(By.Id("start_day")));
                startMonth = wait.Until(x => x.FindElement(By.Id("start_month")));
                startYear = wait.Until(x => x.FindElement(By.Id("start_year")));
                endDay = wait.Until(x => x.FindElement(By.Id("end_day")));
                endMonth = wait.Until(x => x.FindElement(By.Id("end_month")));
                endYear = wait.Until(x => x.FindElement(By.Id("end_year")));
                downloadFormat = wait.Until(x => x.FindElement(By.Id("type")));
            }
            catch (Exception e)
            {
                log.Error("Error finding date elements for share price download at url '" + currentUrl + "' " + e.Message);
                MessageBox.Show("Error in Web Driver", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var elementList = new List<IWebElement>()
            {
                startDay,
                startMonth,
                startYear,
                endDay,
                endMonth,
                endYear,
                downloadFormat
            };

            try
            {
                if(elementList.Contains(null))
                {
                    throw new Exception("List of elements contains null values");
                }

                new SelectElement(startDay).SelectByValue(FormatStringLength(startDate.Day.ToString(), 2));
                new SelectElement(startMonth).SelectByValue(FormatStringLength(startDate.Month.ToString(), 2));
                new SelectElement(startYear).SelectByValue(FormatStringLength(startDate.Year.ToString(), 4));
                new SelectElement(endDay).SelectByValue(FormatStringLength(endDate.Day.ToString(), 2));
                new SelectElement(endMonth).SelectByValue(FormatStringLength(endDate.Month.ToString(), 2));
                new SelectElement(endYear).SelectByValue(FormatStringLength(endDate.Year.ToString(), 4));
                new SelectElement(downloadFormat).SelectByValue("html");

                log.Info("Selected start date: " + startDate + ", end date: " + endDate + " and download format 'html', for share price download at url '" + currentUrl + "'");
            }
            catch(Exception e)
            {
                log.Error("Error setting date elements for share price download at url '" + currentUrl + "' " + e.Message);
                MessageBox.Show("Error in Web Driver", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            

            downloadFormat.Submit();
            try
            {
                ExtractHtmlData(fileName);
            }
            catch(Exception e)
            {
                log.Error("Error extracting download data for url '" + currentUrl + "' " + e.Message);
                MessageBox.Show("Error extracting Web Driver data", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            log.Info("Download form submitted at url '" + currentUrl + "'");
        }

        private static List<List<string>> ExtractHtmlData(string fileName)
        {
            var data = new List<List<string>>();
            var tabs = WebDriver.Driver.WindowHandles;
            WebDriver.Driver.SwitchTo().Window(tabs[tabs.Count - 1]);
            if (WebDriver.Driver.Url.StartsWith("http://digitallook.com") && WebDriver.Driver.Url.EndsWith("html"))
            {
                var tbodyElement = WebDriver.Driver.FindElement(By.TagName("tbody"));
                var tableRows = tbodyElement.FindElements(By.TagName("tr"));
                foreach(var row in tableRows)
                {
                    var fields = row.FindElements(By.TagName("td"));
                    var fieldValues = new List<string>();
                    foreach(var field in fields)
                    {
                        fieldValues.Add(field.Text);
                    }

                    data.Add(fieldValues);
                }
            }

            if (data.Count < 1)
            {
                log.Error("Error extracting data from url '" + WebDriver.Driver.Url + "'");
                MessageBox.Show("Error extracting Web Driver data", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            WriteToFile(data, fileName);
            return data;
        }

        private static void WriteToFile(List<List<string>> data, string fileName)
        {
            string filePath = FilePaths.ShareCastSharePriceDownloadDataDirectory + fileName;
            using (StreamWriter file = new StreamWriter(filePath))
            {
                foreach(var row in data)
                {
                    for (int i = 0; i < row.Count; i++)
                    {
                        if (i == row.Count - 1)
                        {
                            file.Write(row[i]);
                        }
                        else
                        {
                            file.Write(row[i] + ", ");
                        }
                    }

                    file.Write("\n");
                }
            }
        }

        private static string FormatStringLength(string value, int reqiredLength)
        {
            if (value.Length < reqiredLength)
            {
                for (int i = value.Length; i < reqiredLength; i++)
                {
                    value = "0" + value;
                }
            }
                      
            return value;
        }
    }
}

