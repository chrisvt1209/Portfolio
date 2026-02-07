using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace PortfolioApp.Infrastructure.Extensions;

public static class HtmlHelperExtensions
{
    public static bool HasValidationMessageFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression)
    {
        var value = htmlHelper.ValidationMessageFor(expression).GetString();

        return value.Contains("field-validation-error");
    }
}
