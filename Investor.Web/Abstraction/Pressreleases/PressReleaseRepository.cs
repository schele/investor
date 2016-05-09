using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Investor.Models.Models.PressReleases;
using log4net;
using umbraco.NodeFactory;

namespace Investor.Abstraction.Pressreleases
{
    public class PressReleaseRepository : IPressReleaseRepository
    {
        protected ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public XDocument Load(string xmlUrl)
        {
            var xRdr = new XmlTextReader(xmlUrl);

            try
            {
                return XDocument.Load(xRdr);
            }
            catch (Exception)
            {

                return new XDocument();
            }
        }

        public List<PressRelease> GetPressReleases()
        {
            try
            {
                var lang =
                    GetLanguage(
                        umbraco.library.GetCurrentDomains(Node.GetCurrent().Id)[0].Language.
                            CultureAlias);
                //var numberOfPressReleasesToShow = Int32.Parse(CurrentNode.GetProperty("numberOfPressreleases").Value);
                var xmlUrl = string.Format(@"http://ir.investorab.com/?p=press&s=feed&afw_limit=5&afw_lang=" + lang);
                    //((SiteConstants.PRESS_RELEASE_LIST).Replace("%26", "&"), lang);
                var xDoc = Load(xmlUrl);

                return ParsePressReleasesXmlData(xDoc);
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message);

                return new List<PressRelease>();
            }
        }

        public PressRelease GetPressRelease(string id)
        {
            try
            {
                var lang =
                    GetLanguage(
                        umbraco.library.GetCurrentDomains(umbraco.NodeFactory.Node.GetCurrent().Id)[0].Language.
                            CultureAlias);
                var xmlUrl = string.Format("http://ir.investorab.com/?p=reports%26s=feed%26afw_id={0}%26afw_lang={1}".Replace("%26", "&"), id, lang);
                var xDoc = Load(xmlUrl);

                return ParseSinglePressReleaseXmlData(xDoc);
            }
            catch (Exception)
            {
                return new PressRelease();
            }
        }

        public static List<PressRelease> ParsePressReleasesXmlData(XDocument xDoc, int numberOfPressreleasesToShow = 5, string typeToShow = "all")
        {
            var pressReleases = new List<PressRelease>();

            try
            {
                var counter = 0;

                foreach (var mainElement in xDoc.Descendants("release"))
                {
                    if (counter >= numberOfPressreleasesToShow)
                        break;

                    var pressRelease = GetPressRelease(mainElement, true, typeToShow);
                    pressReleases.Add(pressRelease);
                    counter++;
                }
            }

            catch (Exception)
            {
                return new List<PressRelease>();
            }

            return pressReleases;
        }

        public static PressRelease ParseSinglePressReleaseXmlData(XDocument xDoc, string typeToShow = "all")
        {
            try
            {
                PressRelease pressRelease = null;
                
                foreach (var mainElement in xDoc.Descendants("release"))
                {
                    pressRelease = GetPressRelease(mainElement, false, typeToShow);
                }

                return pressRelease;
            }

            catch (Exception)
            {
                return new PressRelease();
            }
        }

        private static PressRelease GetPressRelease(XElement mainElement, bool isSnippet, string typeToShow)
        {
            var body = string.Empty;
            var headline = mainElement.Element("headline").Value;

            if (isSnippet)
            {
                body = GetBodySnippet(mainElement.Element("body").Value);
            }
            else
            {
                body = mainElement.Element("body").Value;
            }

            var id = Int32.Parse(mainElement.Element("id").Value);
            var issuer = mainElement.Element("issuer").Value;
            var published = GetPublishedDate(mainElement.Element("published").Value);
            var modified = published;
            var xmlModified = mainElement.Element("modified").Value;

            if (!string.IsNullOrEmpty(xmlModified))
            {
                modified = GetPublishedDate(xmlModified);
            }

            var attachmentList = new List<Attachment>();

            foreach (var subElement in mainElement.Elements("files").Elements("file"))
            {
                var type = subElement.Attribute("type").Value;

                if (typeToShow == "all" || typeToShow == type)
                {
                    var name = subElement.Element("name").Value;
                    var title = subElement.Element("title").Value;
                    var language = subElement.Element("language").Value;
                    var location = subElement.Element("location").Value;
                    var size = GetSize(subElement.Element("size").Value);

                    if (string.IsNullOrEmpty(title))
                    {
                        title = headline;
                    }

                    var attachment = new Attachment
                    {
                        Name = name,
                        Title = title,
                        Language = language,
                        Location = location,
                        Size = size,
                        Type = type
                    };

                    attachmentList.Add(attachment);
                }
            }

            var pressRelease = new PressRelease
            {
                Headline = headline,
                Body = body,
                Id = id,
                Issuer = issuer,
                Published = published,
                Modified = modified,
                Attachment = attachmentList
            };

            return pressRelease;
        }

        static string GetBodySnippet(string text)
        {
            text = HtmlToText(text);

            return text.Length <= 200 ? text : string.Concat(text.Substring(0, 200), "...");
        }

        static DateTime GetPublishedDate(string date)
        {
            return DateTime.Parse(date);
        }

        static string GetSize(string sizeInBytes)
        {
            if (string.IsNullOrEmpty(sizeInBytes))
            {
                return string.Empty;
            }

            return (Int32.Parse(sizeInBytes) / 1024).ToString(CultureInfo.InvariantCulture) + " KB";
        }

        static string HtmlToText(string text)
        {
            return Regex.Replace(text, "<[^>]*>", String.Empty);
        }

        protected string GetLanguage(string culture)
        {
            return culture.Substring(0, 2);
        }
    }
}