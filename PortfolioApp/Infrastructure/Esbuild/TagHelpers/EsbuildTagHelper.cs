using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PortfolioApp.Infrastructure.Esbuild.TagHelpers;

[HtmlTargetElement("Esbuild-Script")]
public class EsbuildTagHelper : TagHelper
{
    public bool IsDevelopment { get; set; } = false;
    public string? Nonce { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Only render the script in development mode
        if (!IsDevelopment)
        {
            output.SuppressOutput();
            return;
        }

        output.TagName = "script";

        if (!string.IsNullOrEmpty(Nonce))
        {
            output.Attributes.SetAttribute("nonce", Nonce);
        }

        output.Content.SetHtmlContent("""
                                         function connectSSE() {
                                          const source = new EventSource('/sse');

                                          source.onmessage = function (event) {
                                              console.log(event.data);

                                              if (event.data === 'refresh') {
                                                  source.close(); // Close the connection before reload
                                                  location.reload();
                                                  return;
                                              }

                                              try {
                                                  const decodedError = atob(event.data);
                                                  console.error(decodedError);
                                              } catch (error) {
                                                  console.warn("Invalid Base64 message or non-error data:", event.data);
                                              }
                                          };

                                          source.onopen = function(event) {
                                              console.log("Connected to the SSE server");
                                          };

                                          source.onerror = function(event) {
                                              console.log('SSE connection error:', event);
                                              source.close();

                                              // Try to reconnect after 3 seconds
                                              setTimeout(() => {
                                                  console.log("Reconnecting to SSE...");
                                                  connectSSE();
                                              }, 3000);
                                          };
                                      }

                                      connectSSE();

                                      """);

        output.TagMode = TagMode.StartTagAndEndTag;
    }
}
