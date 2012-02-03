using System.Configuration;
using System.Linq;
using System.Web.Mvc;

public static class HtmlHelperExtensions
{
    public static string AppSetting(this HtmlHelper helper, string key)
    {
        return ConfigurationManager.AppSettings.AllKeys.Any(k => k == key)
                   ? ConfigurationManager.AppSettings[key]
                   : string.Empty;
    }
}