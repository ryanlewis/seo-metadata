using Newtonsoft.Json;
using System;
using System.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Epiphany.SeoMetadata
{
    [PropertyValueType(typeof (SeoMetadata))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class SeoMetadataPropertyValueConverter : PropertyValueConverterBase
    {
        private const string RecurseAppSettingKey = "SeoMetadata.RecurseIfDefaults";
        private static readonly bool RecurseIfDefaults;

        static SeoMetadataPropertyValueConverter()
        {
            RecurseIfDefaults = false;
            if (ConfigurationManager.AppSettings[RecurseAppSettingKey] != null)
            {
                Boolean.TryParse(ConfigurationManager.AppSettings[RecurseAppSettingKey], out RecurseIfDefaults);
            }
        }

        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias != null && propertyType.PropertyEditorAlias.Equals("Epiphany.SeoMetadata");
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null) return null;
            var sourceString = source.ToString();
            if (String.IsNullOrWhiteSpace(sourceString)) return null;

            try
            {
                var md = JsonConvert.DeserializeObject<SeoMetadata>(sourceString);

                if (RecurseIfDefaults &&
                    String.IsNullOrWhiteSpace(md.Title) &&
                    String.IsNullOrWhiteSpace(md.Description) &&
                    String.IsNullOrWhiteSpace(md.UrlName))
                {
                    return null;
                }

                return md;
            }
            catch (Exception e)
            {
                LogHelper.Warn<SeoMetadataPropertyValueConverter>(String.Format("Cannot deserialize SeoMetadata - {0} - {1}",
                    e.GetType().Name, e.Message));
                return null;
            }
        }
    }
}