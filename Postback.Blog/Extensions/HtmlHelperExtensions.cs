using Postback.Blog.Models.ViewModels;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

public static class HtmlHelperExtensions
{
    public static string AppSetting(this HtmlHelper helper, string key)
    {
        return ConfigurationManager.AppSettings.AllKeys.Any(k => k == key)
                   ? ConfigurationManager.AppSettings[key]
                   : string.Empty;
    }

    public static MvcHtmlString Pager(this HtmlHelper helper, PagingView paging, string template = "_paging") {
        if (paging == null)
        {
            return new MvcHtmlString("No paging found in ViewBag");
        }
        var routeData = helper.ViewContext.RouteData;
        var controller = routeData.GetRequiredString("controller").ToNullSafeString();
        var action = routeData.GetRequiredString("action").ToNullSafeString();
        var area = routeData.DataTokens.ContainsKey("area") ? routeData.DataTokens["area"] + "/" : string.Empty;

        paging.Uri = () => "/" + area.ToLower() + controller.ToLower() + "/" + action.ToLower();
        return helper.Partial(template, paging);
    }
}