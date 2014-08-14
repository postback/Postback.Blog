using Microsoft.Practices.ServiceLocation;
using Postback.Blog.App.DependencyResolution;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Postback.Blog.App.Bootstrap
{
    public class StartupModule : BaseHttpModule
    {
        private static bool hasBeenStarted;
        private static readonly object Lock = new object();

        public override void OnBeginRequest(HttpContextBase context)
        {
            if (!hasBeenStarted)
            {
                lock (Lock)
                {
                    if (!hasBeenStarted)
                    {
                        ObjectFactory.ResetDefaults();
                        ConfigureOnStartup();
                        hasBeenStarted = true;
                    }
                }
            }
        }

        public void ConfigureOnStartup()
        {
            var startUpTasks = ServiceLocator.Current.GetAllInstances<IStartUpTask>();

            foreach (var task in startUpTasks)
                task.Configure();
        }

        //Only for unit testing purposes
        public static void Reset()
        {
            hasBeenStarted = false;
        }
    }
}