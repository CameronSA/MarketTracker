namespace MarketTracker.Commands
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using MarketTracker.ViewModels;
    using MarketTracker.WebDriver;
    using MarketTracker.WebDriver.WebsiteProtocols.ShareCast.Procedures;

    public class ShareCastSharePriceCommand : ICommand
    {
        public ShareCastSharePriceCommand(ShareCastSharePriceViewModel viewModel, ApplicationViewModel applicationViewModel)
        {
            this.ViewModel = viewModel;
            this.ApplicationViewModel = applicationViewModel;
        }

        public ShareCastSharePriceViewModel ViewModel { get; set; }

        public ApplicationViewModel ApplicationViewModel { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            bool canExecute = false;
            switch(parameter.ToString())
            {
                case "btStart":
                    canExecute = true;
                    break;
            }

            return canExecute;
        }

        public void Execute(object parameter)
        {
            switch(parameter.ToString())
            {
                case "btStart":
                    this.StartDriver();
                    break;                    
            }
        }

        private void StartDriver()
        {
            new Thread(new ThreadStart(DriverThread)).Start();
        }

        private void DriverThread()
        {
            if (WebDriver.Initialise(this.ApplicationViewModel))
            {
                if (WebDriver.Setup(this.ViewModel.Model.FtseIndex))
                {
                    ShareCastSharePrices.Download(
                        this.ViewModel.Model.AllCompaniesSelected,
                        this.ViewModel.Model.AllRecords,
                        this.ViewModel.Model.SelectedCompany,
                        this.ViewModel.Model.StartDate,
                        this.ViewModel.Model.EndDate);
                }

                WebDriver.Dispose();
            }
        }
    }
}
