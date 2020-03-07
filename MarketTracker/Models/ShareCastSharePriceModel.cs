namespace MarketTracker.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;

    public class ShareCastSharePriceModel : INotifyPropertyChanged
    {
        private bool isFtse100;

        private bool isFtse250;

        private int ftseIndex;

        private string selectedCompany;

        private bool allCompaniesSelected;

        public bool IsFtse100
        {
            get
            {
                return this.isFtse100;
            }

            set
            {
                this.isFtse100 = value;
                this.OnPropertyChanged("IsFtse100");
            }
        }

        public bool IsFtse250
        {
            get
            {
                return this.isFtse250;
            }

            set
            {
                this.isFtse250 = value;
                this.OnPropertyChanged("IsFtse250");
            }
        }

        public int FtseIndex
        {
            get
            {
                return this.ftseIndex;
            }

            set
            {
                this.ftseIndex = value;
                this.OnPropertyChanged("FtseIndex");
                if(this.ftseIndex == 100)
                {
                    this.IsFtse100 = true;
                    this.IsFtse250 = false;
                }

                if(this.ftseIndex == 250)
                {
                    this.IsFtse100 = false;
                    this.IsFtse250 = true;
                }
            }
        }

        public bool AllCompaniesSelected
        {
            get
            {
                return this.allCompaniesSelected;
            }

            set
            {
                this.allCompaniesSelected = value;
                this.OnPropertyChanged("AllCompaniesSelected");
            }
        }

        public string SelectedCompany
        {
            get
            {
                return this.selectedCompany;
            }

            set
            {
                this.selectedCompany = value;
                this.OnPropertyChanged("SelectedCompany");
            }
        }

        public List<string> CompanyNames100 { get; set; }
        public List<string> CompanyNames250 { get; set; }

        public List<int> FtseIndexes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(object property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property.ToString()));
        }
    }
}
