using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;
using Microsoft.Practices.ServiceLocation;

namespace Postback.Blog.App.DependencyResolution
{
    public class ServiceLocatorDependencyResolver : IDependencyResolver
    {
        public ServiceLocatorDependencyResolver()
        {
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null) return null;
            try
            {
                if (serviceType.IsAbstract || serviceType.IsInterface)
                {
                    return ServiceLocator.Current.GetInstance(serviceType);
                }
                else
                {
                    return ServiceLocator.Current.GetInstance(serviceType);
                }
            }
            catch 
            {

                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ServiceLocator.Current.GetAllInstances<object>().Where(s => s.GetType() == serviceType);
        }
    }
}