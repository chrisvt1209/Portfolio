using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace PortfolioApp.Infrastructure.Helpers;

public partial class HtmlHelper
{
    private readonly string _baseUrl;
    private readonly IWebHostEnvironment _hostEnvironment;

    [GeneratedRegex("#body#", RegexOptions.IgnoreCase, "en-NL")]
    private static partial Regex BodyTemplateRegex();

    public HtmlHelper(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _baseUrl = configuration.GetValue<string>("Umbraco:CMS:WebRouting:UmbracoApplicationUrl") ?? "";
    }

    public async Task<string> WrapHtmlEmailTemplate(string emailBody, string variantName)
    {
        var htmlBody = await File.ReadAllTextAsync($"{_hostEnvironment.WebRootPath.TrimEnd('/')}/emailtemplates/{variantName}");
        htmlBody = ConvertRelativeLinksToAbsolute(htmlBody);
        htmlBody = htmlBody.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        emailBody = ConvertRelativeLinksToAbsolute(emailBody);
        return BodyTemplateRegex().Replace(htmlBody, emailBody);
    }

    public string ConvertRelativeLinksToAbsolute(string? htmlString)
    {
        if (htmlString == null)
        {
            return "";
        }

        var doc = new HtmlDocument();
        doc.LoadHtml(htmlString);

        ProcessTags(doc, "//img[@src]", "src");
        ProcessTags(doc, "//a[@href]", "href");

        return doc.DocumentNode.OuterHtml;
    }


    private void ProcessTags(HtmlDocument doc, string xpath, string attributeName)
    {
        var tags = doc.DocumentNode.SelectNodes(xpath);
        if (tags == null)
        {
            return;
        }

        foreach (var tag in tags)
        {
            var attributeValue = tag.GetAttributeValue(attributeName, "");
            if (string.IsNullOrEmpty(attributeValue) ||
                Uri.IsWellFormedUriString(attributeValue, UriKind.Absolute) ||
                attributeValue.StartsWith('{'))
            {
                continue;
            }

            var absoluteUrl = new Uri(new Uri(_baseUrl), attributeValue).AbsoluteUri;
            tag.SetAttributeValue(attributeName, absoluteUrl);
        }
    }
}
