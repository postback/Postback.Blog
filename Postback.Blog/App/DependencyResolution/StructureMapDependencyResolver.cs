using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace Postback.Blog.App.DependencyResolution
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer container;

        public StructureMapDependencyResolver(IContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null) return null;
            try
            {
                if (serviceType.IsAbstract || serviceType.IsInterface)
                {
                    return container.TryGetInstance(serviceType);
                }
                else
                {
                    return container.GetInstance(serviceType);
                }
            }
            catch 
            {

                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetAllInstances<object>().Where(s => s.GetType() == serviceType);
        }
    }
}