﻿using Microsoft.Practices.ServiceLocation;
using Postback.Blog.App.DependencyResolution;
using StructureMap;

namespace Postback.Blog.App.Bootstrap
{
    //Depends on StructureMap. Does this need to be decoupled?
    public class Bootstrapper
    {
        static Bootstrapper()
        {
            DependencyRegistrar.EnsureDependenciesRegistered();
        }

        public static void StartUp()
        {
            var startUpTasks = ServiceLocator.Current.GetAllInstances<IStartUpTask>();

            foreach (var task in startUpTasks)
                task.Configure();
        }
    }
}