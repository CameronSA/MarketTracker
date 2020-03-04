namespace MarketTracker.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class MainWindowModel : INotifyPropertyChanged
    {
        private int ftseIndex;

        public List<int> FTSEIndexes { get; set; }

        public int FTSEIndex
        {
            get
            {
                return this.ftseIndex;
            }

            set
            {
                this.ftseIndex = value;
                this.OnPropertyChanged("FTSEIndex");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;  
        protected void OnPropertyChanged(object property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property.ToString()));
        }
    }
}
