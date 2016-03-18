using System;
using System.Configuration;
using System.Linq;
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
                // TODO: Vorto segment provider didn't used to work due to culture issues within this scope. Needs further testing in more recent umb versions
                var providerType = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == "Our.Umbraco.Vorto")
                    ? typeof (VortoSeoMetadataUrlSegmentProvider)
                    : typeof (SeoMetadataUrlSegmentProvider);

                UrlSegmentProviderResolver.Current.InsertTypeBefore(typeof(DefaultUrlSegmentProvider), providerType);
                LogHelper.Info<SeoMetadataStartup>("Configured SeoMetadataUrlSegmentProvider");
            }

            base.ApplicationStarting(umbracoApplication, applicationContext);
        }
    }
}