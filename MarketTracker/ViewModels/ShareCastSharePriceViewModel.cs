namespace MarketTracker.ViewModels
{
    using MarketTracker.Commands;
    using MarketTracker.Data;
    using MarketTracker.Models;
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    public class ShareCastSharePriceViewModel
    {
        public ShareCastSharePriceViewModel(ApplicationViewModel applicationViewModel)
        {
            this.Model = new ShareCastSharePriceModel
            {
                CompanyNames100 = new List<string>(),
                CompanyNames250 = new List<string>(),
                FtseIndex = Default.FTSEIndexes[0],
                FtseIndexes = Default.FTSEIndexes,
                EndDate = DateTime.Today,
                StartDate = new DateTime(2009,01,01)
            };

            this.Command = new ShareCastSharePriceCommand(this, applicationViewModel);

            this.ConfigureViewModel();
        }

        public ShareCastSharePriceModel Model { get; }

        public ICommand Command { get; }

        private void ConfigureViewModel()
        {
            foreach(var company in WebDriver.WebDriverData.ShareCastCompanies100)
            {
                this.Model.CompanyNames100.Add(company.Name);
            }

            foreach(var company in WebDriver.WebDriverData.ShareCastCompanies250)
            {
                this.Model.CompanyNames250.Add(company.Name);
            }
        }
    }
}
