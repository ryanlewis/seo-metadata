using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Strings;
using Umbraco.Web;

namespace Epiphany.SeoMetadata
{
    public class VortoSeoMetadataUrlSegmentProvider : IUrlSegmentProvider
    {
        private static readonly string PropertyName = "metadata";

        static VortoSeoMetadataUrlSegmentProvider()
        {
            var propertyName = ConfigurationManager.AppSettings["SeoMetadata.PropertyName"];
            if (!String.IsNullOrWhiteSpace(propertyName))
            {
                PropertyName = propertyName;
            }

            // Get the VortoValue type 
            //var assembly = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Our.Umbraco.Vorto");
            //_vortoValueType = assembly.GetType("VortoValue");
        }

        public string GetUrlSegment(IContentBase content)
        {
            if (!content.HasProperty(PropertyName)) return null;
            return GetUrlSegment(content, null);
        }

        public string GetUrlSegment(IContentBase content, CultureInfo cultureInfo)
        {
            //var culture = cultureInfo == null ? Thread.CurrentThread.CurrentUICulture.Name : cultureInfo.Name;
            var culture = GetCulture(content);

            try
            {
                var value = content.GetValue<string>(PropertyName);

                var definition = new { Values = new Dictionary<string, string>() };
                var vortoValue = JsonConvert.DeserializeAnonymousType(value, definition);

                var metadataJson = vortoValue.Values[culture.Name];
                var metadata = JsonConvert.DeserializeObject<SeoMetadata>(metadataJson);

                if (metadata == null || String.IsNullOrWhiteSpace(metadata.UrlName)) return null;
                return metadata.UrlName.ToUrlSegment();
            }
            catch
            {
                return null;
            }
        }

        private static CultureInfo GetCulture(IContentBase content)
        {
            // get the ID of what we want to look at for the culture
            var id = content.HasIdentity ? content.Id : content.ParentId;

            var helper = new UmbracoHelper(UmbracoContext.Current);
            var publishedDoc = helper.TypedContent(id);
            var culture = publishedDoc.GetCulture();

            return culture ?? Thread.CurrentThread.CurrentUICulture;
        }
    }
}
