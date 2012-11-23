using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public static class UrlHelperExtensions
{
    public static bool Is(this UrlHelper helper, string controller, string action)
    {
        var routeValueDictionary = helper.RequestContext.RouteData.Values;
        return routeValueDictionary["controller"].ToNullSafeString().ToLower() == controller.ToLower() && routeValueDictionary["action"].ToNullSafeString().ToLower() == action.ToLower();
    }
}