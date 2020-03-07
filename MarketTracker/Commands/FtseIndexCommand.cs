namespace MarketTracker.Commands
{
    using MarketTracker.ViewModels;
    using MarketTracker.WebDriver;
    using System;
    using System.Threading;
    using System.Windows.Input;

    public class FtseIndexCommand : ICommand
    {
        public FtseIndexCommand(FtseIndexViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public FtseIndexViewModel ViewModel { get; private set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            bool canExecute = false;
            switch (parameter.ToString())
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
            if (WebDriver.Initialise(this.ViewModel.ApplicationViewModel))
            {
                if (WebDriver.Setup(this.ViewModel.Model.FTSEIndex))
                {
                    WebDriver.RunSetupProcedures("sharecast");
                }

                WebDriver.Dispose();
            }
        }
    }
}
