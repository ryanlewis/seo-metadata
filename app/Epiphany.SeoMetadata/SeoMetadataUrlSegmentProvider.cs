using System;
using System.Globalization;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Strings;

namespace Epiphany.SeoMetadata
{
    public class SeoMetadataUrlSegmentProvider : IUrlSegmentProvider
    {
        public string GetUrlSegment(IContentBase content)
        {
            if (!content.HasProperty("metadata")) return null;

            try
            {
                var metadata = JsonConvert.DeserializeObject<SeoMetadata>(content.GetValue<string>("metadata"));
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