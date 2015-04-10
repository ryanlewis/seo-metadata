using Umbraco.Core;
using Umbraco.Core.Strings;

namespace Epiphany.SeoMetadata
{
    public class SeoMetadataStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            UrlSegmentProviderResolver.Current.InsertTypeBefore(typeof(DefaultUrlSegmentProvider), typeof(SeoMetadataUrlSegmentProvider));

            base.ApplicationStarting(umbracoApplication, applicationContext);
        }
    }
}