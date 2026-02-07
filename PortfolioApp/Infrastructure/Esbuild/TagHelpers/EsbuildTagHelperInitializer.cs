using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PortfolioApp.Infrastructure.Esbuild.TagHelpers;

[HtmlTargetElement("Esbuild-Script")]
public class EsbuildTagHelperInitializer : ITagHelperInitializer<EsbuildTagHelper>
{
    private bool IsDevelopment { get; set; }

    public EsbuildTagHelperInitializer(bool isDevelopment)
    {
        IsDevelopment = isDevelopment;
    }

    public void Initialize(EsbuildTagHelper helper, ViewContext context)
    {
        helper.IsDevelopment = IsDevelopment;
    }
}
