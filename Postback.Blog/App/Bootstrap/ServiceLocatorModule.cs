using System;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using Postback.Blog.App.DependencyResolution;

namespace Postback.Blog.App.Bootstrap
{
    public class ServiceLocatorModule : BaseHttpModule
    {
        public Action SetLocatorProvider = () => ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator());

        public override void OnBeginRequest(HttpContextBase context)
        {
            SetLocatorProvider();
        }
    }
}