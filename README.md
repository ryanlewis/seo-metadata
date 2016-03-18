![SEO metadata](https://raw.githubusercontent.com/ryanlewis/seo-metadata/master/images/epiphany-logo.png)

# SEO Metadata for Umbraco

SEO Metadata for Umbraco is a property editor that is used for maintaining common SEO-related information for a page. It gives users a visual representation of how the page would look on a Google search result page and hints to when the title and description is too long, with optional validation.

![SEO metadata](https://raw.githubusercontent.com/ryanlewis/seo-metadata/master/images/example1.gif)

## Recent Changes

**0.3.0**

* Allow for recursive property values if all the fields are blank and the AppSettingKey is set.

**0.2.1**

* Fixed some issues with Umbraco package
* Resolved issue with PropertyEditorValueConverter returning null if page is saved when control is unused

**0.2.0**

* Fixed issue with custom URL Names not working if `SeoMetadata.NoSegmentProvider` appSetting wasn't present
* Added new option to set the developer name
 
**0.1.0**

* Initial release


## Installation

Install the latest version through NuGet.
```
Install-Package Epiphany.SeoMetadata
```

After installing via Nuget, create a property editor of type **SEO Metadata** and include on your page. We recommend the property name **"metadata"** to work with all features out-of-the-box (see the [URL Name](#using-the-url-name) section for configuration options)

![Property editor options](https://raw.githubusercontent.com/ryanlewis/seo-metadata/master/images/property-editor-options.png)

Alternatively, if you want to hack around with the project, you can fork, checkout and develop locally. See the [Developing SEO Metadata](#developing-seo-metadata) section.

## Configuration

<dl>
    <dt>Allow long titles</dt>
    <dd>If ticked, long titles will not invalidate the property editor.</dd>
    
    <dt>Allow long descriptions</dt>
    <dd>If ticked, long descriptions will not invalidate the property editor.</dd>
    
    <dt>SERP Title Length</dt>
    <dd>The maximum length of a title. This isn't an exact number, so your mileage may vary. The default value of 65 is a conservative value that should work for most cases. Google will truncate overly long titles with ellipses (&hellip;)</dd>
    
    <dt>SERP Description Length</dt>
    <dd>The maximum length of the description. This isn't an exact number, so your mileage may vary. The default value of 150 is a conservative value that should work for most cases. Google will truncate overly long descriptions with ellipses (&hellip;)   </dd>
    
    <dt>Developer Name</dt>
    <dd>Allows you to personalise the template a bit by putting the name of your company/agency/other. This is used within the descriptions on the view and is displayed to your content editors. </dd>
</dl>

## Usage

The SEO Metadata is stored as JSON, so can be used dynamically.

```c#
<pre>
Title:         @CurrentPage.Metadata.Title
Description:   @CurrentPage.Metadata.Description
Do Not Index?: @CurrentPage.Metadata.NoIndex
URL Name:      @CurrentPage.Metadata.UrlName
</pre>
```

A [Property Editor Value Converter][1] is installed for getting a strongly-typed **SeoMetadata** instance.

```c#
@{
    var metadata = Model.Content.GetPropertyValue<Epiphany.SeoMetadata.SeoMetadata>("metadata");
}

<pre>
Title:         @metadata.Title
Description:   @metadata.Description
Do Not Index?: @metadata.NoIndex
URL Name:      @metadata.UrlName
</pre>
```

The following snippet can be used for using the **Do Not Index** checkbox.

```c#
@if (Model.Content.GetPropertyValue<Epiphany.SeoMetadata.SeoMetadata>("metadata").NoIndex)
{
    <meta name="robots" content="noindex">
}
```

If you're a fan of [ZpqrtBnk Umbraco Models Builder][2], you can add something like the following in your partial class

```c#
[ImplementPropertyType("metadata")]
public virtual SeoMetadata Metadata
{
    get { return this.GetPropertyValue<SeoMetadata>("metadata"); }
}
```

### Using the URL Name

SEO Metadata also installs a [UrlSegmentProvider][3] to ensure the URL Name property works as intended. By default, it expects your SEO Metadata property to be called `metadata`. You can configure this property by adding the following setting to your appSettings in your web.config

```xml
<add key="SeoMetadata.PropertyName" value="seoMetadata" />
```

If you want to disable the `SeoMetadataUrlSegmentProvider` altogether (to add manually, or implement yourself), you can set the following appSetting to disable it.
```xml
<add key="SeoMetadata.NoSegmentProvider" value="true" />
```

If fields are left blank you want the values from the parent to be resolved, add the following appSetting key.
```xml
<add key="SeoMetadata.RecurseIfDefaults" value="true" />
```

[1]:https://our.umbraco.org/documentation/extending-umbraco/Property-Editors/PropertyEditorValueConverters
[2]:https://github.com/zpqrtbnk/Zbu.ModelsBuilder
[3]:https://our.umbraco.org/documentation/Reference/Request-Pipeline/outbound-pipeline#segments

## Developing SEO Metadata

### Checkout the project
```bash
git clone https://github.com/ryanlewis/seo-metadata.git
cd seo-metadata
```

### Install Dependencies

```bash
npm install -g grunt-cli
npm install
```

### Build

```bash
build.cmd
grunt
```

If you wish to build it to a local Umbraco directory, use the `target` option.

```bash
grunt --target=c:\dev\path-to-umbraco-root-dir
```
