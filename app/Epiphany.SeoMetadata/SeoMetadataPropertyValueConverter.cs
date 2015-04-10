using System;
using Newtonsoft.Json;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Epiphany.SeoMetadata
{
    [PropertyValueType(typeof(SeoMetadata))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class SeoMetadataPropertyValueConverter : PropertyValueConverterBase
    {
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
                return JsonConvert.DeserializeObject<SeoMetadata>(sourceString);
            }
            catch (Exception e)
            {
                LogHelper.Warn<SeoMetadataPropertyValueConverter>(String.Format("Cannot deserialize SeoMetadata - {0} - {1}", e.GetType().Name, e.Message));
                return null;
            }
        }
    }
}