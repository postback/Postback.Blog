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
        var routeData = helper.ViewContext.RouteData;
        var controller = routeData.GetRequiredString("controller");
        var action = routeData.GetRequiredString("action");
        var area = routeData.Values.ContainsKey("area") ?  routeData.Values["area"] + "/" : string.Empty;

        paging.Uri = () => area + controller + "/" + action;
        return helper.Partial(template, paging);
    }
}