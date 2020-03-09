namespace MarketTracker.Models
{
    using System;
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

        private DateTime startDate;

        private DateTime endDate;

        private bool startDateIsToday;

        private bool endDateIsToday;

        private bool allRecords;

        public bool AllRecords
        {
            get
            {
                return this.allRecords;
            }

            set
            {
                this.allRecords = value;
                this.OnPropertyChanged("AllRecords");
            }
        }

        public bool StartDateIsToday
        {
            get
            {
                return this.startDateIsToday;
            }

            set
            {
                this.startDateIsToday = value;
                this.OnPropertyChanged("StartDateIsToday");
                if(value)
                {
                    this.StartDate = DateTime.Today;
                }
            }
        }

        public bool EndDateIsToday
        {
            get
            {
                return this.endDateIsToday;
            }

            set
            {
                this.endDateIsToday = value;
                this.OnPropertyChanged("EndDateIsToday");
                if(value)
                {
                    this.EndDate = DateTime.Today;
                }
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }

            set
            {
                this.startDate = value;
                this.OnPropertyChanged("StartDate");
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.endDate;
            }

            set
            {
                this.endDate = value;
                this.OnPropertyChanged("EndDate");
            }
        }

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
