namespace MarketTracker.Commands
{
    using MarketTracker.ViewModels;
    using MarketTracker.WebDriver;
    using System;
    using System.Windows.Input;

    public class MainWindowCommand : ICommand
    {
        public MainWindowCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public MainWindowViewModel ViewModel { get; private set; }

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
            var driver = new WebDriver();            
            if(driver.Setup(this.ViewModel.Model.FTSEIndex))
            {
                driver.RunProcedures("sharecast");
            }

            driver.Dispose();
        }
    }
}
