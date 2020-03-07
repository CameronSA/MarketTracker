namespace MarketTracker.WebDriver.WebsiteProtocols.ShareCast.Procedures
{
    using System;
    using System.Threading;
    using System.Windows;
    using MarketTracker.WebDriver.Objects;

    public static class ShareCastSharePrices
    {
        public static void Download(bool getFullRange, DateTime startDate = new DateTime(), DateTime endDate = new DateTime())
        {
            if (WebDriverData.ShareCastCompanies100.Count < 1)
            {
                if (!WebDriverData.PopulateProperties())
                {
                    return;
                }
            }

            Company company = WebDriverData.ShareCastCompanies100[0];
            string companyUrl = company.Url + "/share-prices/download";
            WebDriver.Navigate(companyUrl);
            Thread.Sleep(100);
        }
    }
}
