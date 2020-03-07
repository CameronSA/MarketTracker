namespace MarketTracker.Models
{
    using System.ComponentModel;
    using System.Windows;

    public class ApplicationModel : INotifyPropertyChanged
    {
        private bool driverRunning;

        public bool DriverRunning
        {
            get
            {
                return this.driverRunning;
            }

            set
            {
                this.driverRunning = value;
                this.OnPropertyChanged("DriverRunning");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(object property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property.ToString()));
        }
    }
}
