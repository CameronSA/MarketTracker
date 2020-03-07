namespace MarketTracker.ViewModels
{
    using MarketTracker.Commands;
    using MarketTracker.Data;
    using MarketTracker.Models;
    using System.Collections.Generic;
    using System.Windows.Input;

    public class FtseIndexViewModel
    {
        public FtseIndexViewModel(ApplicationViewModel viewModel)
        {
            this.Model = new FtseIndexModel
            {
                FTSEIndexes = new List<int>()
            };

            foreach (var index in Default.FTSEIndexes)
            {
                this.Model.FTSEIndexes.Add(index);
            }

            this.Model.FTSEIndex = this.Model.FTSEIndexes[0];
            this.Command = new FtseIndexCommand(this);
            this.ApplicationViewModel = viewModel;
        }

        public ApplicationViewModel ApplicationViewModel { get; }

        public FtseIndexModel Model { get; private set; }

        public ICommand Command { get; private set; }
    }
}
