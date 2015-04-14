using System;
using System.Configuration;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Strings;

namespace Epiphany.SeoMetadata
{
    public class SeoMetadataStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            bool ignoreSegmentProvider;

            var hasKey = Boolean.TryParse(ConfigurationManager.AppSettings["SeoMetadata.NoSegmentProvider"], out ignoreSegmentProvider);

            if (!hasKey || !ignoreSegmentProvider)
            {
                UrlSegmentProviderResolver.Current.InsertTypeBefore(typeof(DefaultUrlSegmentProvider), typeof(SeoMetadataUrlSegmentProvider));
                LogHelper.Info<SeoMetadataStartup>("Configured SeoMetadataUrlSegmentProvider");
            }

            base.ApplicationStarting(umbracoApplication, applicationContext);
        }
    }
}