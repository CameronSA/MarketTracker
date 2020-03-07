namespace MarketTracker.WebDriver
{
    using log4net;
    using MarketTracker.Data;
    using MarketTracker.WebDriver.Objects;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows;

    public static class WebDriverData
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, string> MarketSites { get; set; } = new Dictionary<string, string>();

        public static int FTSEIndex { get; set; }

        public static List<Company> ShareCastCompanies100 { get; set; } = new List<Company>();

        public static List<Company> ShareCastCompanies250 { get; set; } = new List<Company>();

        public static bool PopulateProperties()
        {
            ParseShareCastCompanies(ShareCastCompanies100, FilePaths.ShareCastCompanies100);
            ParseShareCastCompanies(ShareCastCompanies250, FilePaths.ShareCastCompanies250);
            if (ShareCastCompanies100.Count < 1 || ShareCastCompanies250.Count < 1)
            {
                MessageBox.Show("Some basic company data is missing. Please run setup functions", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private static void ParseShareCastCompanies(List<Company> property, string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    string[] lineElements = line.Split(',');
                    if (lineElements.Length != 2)
                    {
                        MessageBox.Show("Error parsing file '" + filePath + "'", "Market Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    property.Add(new Company()
                    {
                        Name = lineElements[0],
                        Url = lineElements[1]
                    });
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex.Message);
            }
        }
    }
}
