using System;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Strings;

namespace Epiphany.SeoMetadata
{
    public class SeoMetadataUrlSegmentProvider : IUrlSegmentProvider
    {
        private static readonly string PropertyName = "metadata";

        static SeoMetadataUrlSegmentProvider()
        {
            var propertyName = ConfigurationManager.AppSettings["SeoMetadata.PropertyName"];
            if (!String.IsNullOrWhiteSpace(propertyName))
            {
                PropertyName = propertyName;
            }
        }

        public string GetUrlSegment(IContentBase content)
        {
            if (!content.HasProperty(PropertyName)) return null;

            try
            {
                var metadata = JsonConvert.DeserializeObject<SeoMetadata>(content.GetValue<string>(PropertyName));
                if (metadata == null || String.IsNullOrWhiteSpace(metadata.UrlName)) return null;
                return metadata.UrlName.ToUrlSegment();
            }
            catch
            {
                return null;
            }
        }

        public string GetUrlSegment(IContentBase content, CultureInfo culture)
        {
            return GetUrlSegment(content);
        }
    }
}