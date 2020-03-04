namespace MarketTracker.WebDriver
{
    using System.Collections.Generic;

    public static class WebDriverData
    {
        public static Dictionary<string, string> MarketLinks { get; set; } = new Dictionary<string, string>();
        
        public static int FTSEIndex { get; set; }
    }
}
