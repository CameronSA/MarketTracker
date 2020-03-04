namespace MarketTracker.WebDriver
{
    using log4net;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

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

        public IWebDriver Driver { get; }

        public void Navigate(KeyValuePair<string, string> website)
        {
            this.Driver.Navigate().GoToUrl(website.Value);               
        }

        public void Dispose()
        {
            this.Driver.Quit();
            this.Driver.Dispose();
        }
    }
}
