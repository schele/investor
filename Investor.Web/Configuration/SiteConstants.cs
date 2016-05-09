using System.Configuration;

namespace Investor.Configuration
{
    public static class SiteConstants
    {
        public static string LatestAnnualReport
        {
            get { return ConfigurationManager.AppSettings["latest-annual-report"]; }
        }

        public static string LatestInterimReport
        {
            get { return ConfigurationManager.AppSettings["latest-interim-report"]; }
        }

        public static string CustomQuote
        {
            get { return ConfigurationManager.AppSettings["custom-quote"]; }
        }
    }
}