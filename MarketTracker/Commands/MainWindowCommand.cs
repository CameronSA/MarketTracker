namespace MarketTracker.Commands
{
    using MarketTracker.WebDriver;
    using MarketTracker.ViewModels;
    using System;
    using System.Windows;
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
                    this.Start();
                    break;
            }
        }

        private void Start()
        {

        }
    }
}
