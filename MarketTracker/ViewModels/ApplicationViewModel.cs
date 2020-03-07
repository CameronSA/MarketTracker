namespace MarketTracker.ViewModels
{
    using MarketTracker.Models;
    using MarketTracker.WebDriver;

    public class ApplicationViewModel
    {
        public ApplicationViewModel()
        {
            this.Model = new ApplicationModel
            {
                DriverRunning = WebDriver.DriverRunning
            };
        }

        public ApplicationModel Model { get; private set; }
    }
}
