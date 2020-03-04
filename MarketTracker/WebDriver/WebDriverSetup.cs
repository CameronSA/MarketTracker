namespace MarketTracker.WebDriver
{
    using log4net;
    using MarketTracker.Data;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows;

    public class WebDriverSetup
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public WebDriverSetup(int ftseIndex)
        {
            WebDriverData.FTSEIndex = ftseIndex;
            this.Successful = false;
            try
            {
                this.ParseMarketSitesFile();
                this.Successful = true;
            }
            catch
            {
                MessageBox.Show("Unable to parse file located at '" + FilePaths.MarketSites + "'");
            }
        }

        public bool Successful { get; private set; }

        private void ParseMarketSitesFile()
        {
            string[] fileLines = File.ReadAllLines(FilePaths.MarketSites);
            foreach (var fileLine in fileLines)
            {
                string[] fields = fileLine.Split(',');
                if (fields.Length != 2)
                {
                    log.Error(string.Format("Unable to parse file located at '{0}'\n\tExpected 2 fields, parsed {1}", FilePaths.MarketSites, fields.Length));

                    throw new Exception();
                }

                if (!WebDriverData.MarketLinks.ContainsKey(fields[0]))
                {
                    bool parsed = int.TryParse(fields[1].Substring(fields[1].Length - 3, 3), out int index);

                    if (index != WebDriverData.FTSEIndex)
                    {
                        if (parsed && Default.FTSEIndexes.Contains(index))
                        {
                            string initialUrl = fields[1];
                            fields[1] = fields[1].Remove(fields[1].Length - 3, 3);
                            fields[1] += WebDriverData.FTSEIndex.ToString();
                            log.Info(string.Format("Updated url: '{0}' to '{1}' with ftse index: {2}", initialUrl, fields[1], WebDriverData.FTSEIndex.ToString()));
                        }
                        else
                        {
                            log.Warn(string.Format("Cannot find ftse index in url: '{0}'. Index not updated", fields[1]));
                        }
                    }

                    WebDriverData.MarketLinks.Add(fields[0].Trim().ToLower(), fields[1].Trim());
                }
            }
        }
    }
}
