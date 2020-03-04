namespace MarketTracker.ViewModels
{
    using MarketTracker.Commands;
    using MarketTracker.Data;
    using MarketTracker.Models;
    using System.Collections.Generic;
    using System.Windows.Input;

    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            this.Model = new MainWindowModel();
            this.Model.FTSEIndexes = new List<int>();
            foreach(var index in Default.FTSEIndexes)
            {
                this.Model.FTSEIndexes.Add(index);
            }

            this.Model.FTSEIndex = this.Model.FTSEIndexes[0];
            this.Command = new MainWindowCommand(this);
        }

        public MainWindowModel Model { get; private set; }

        public ICommand Command { get; private set; }
    }
}
