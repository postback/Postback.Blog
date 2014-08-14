using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;
using Microsoft.Practices.ServiceLocation;
using StructureMap;
using StructureMap.Graph;
using Postback.Blog.App.Bootstrap;

namespace Postback.Blog.App.DependencyResolution
{
    public class StructureMapServiceLocator : IServiceLocator
    {
        public Action<Type> Initialize = (type) =>
        {
            string assemblyPrefix = GetAssembliesPrefix(type);

            ObjectFactory.Initialize(x => x.Scan(y =>
            {
                ////TODO: Is this a duplicate from the AssembliesFromApplicationBaseDirectory part?
                //var referencesAssemblies = type.Assembly.GetReferencedAssemblies();
                //IEnumerable<AssemblyName> enumerable = referencesAssemblies.Where(name => name.Name.StartsWith(assemblyPrefix));
                //enumerable.Each(name => y.Assembly(name.Name));//Load all referenced assemblies

                y.AssembliesFromApplicationBaseDirectory(z => z.GetName().Name.StartsWith(assemblyPrefix));

                y.TheCallingAssembly();
                y.AssemblyContainingType<StructureMapServiceLocator>();

                y.LookForRegistries();

                y.AddAllTypesOf<IStartUpTask>();

                y.Convention<DefaultConventionScanner>();//Maps things like IAnything to Anything
            }));
        };
        private static bool dependenciesRegistered;
        private static readonly object sync = new object();

        public StructureMapServiceLocator()
        {
            EnsureDependenciesRegistered();
        }

        //Only for test purposes
        public void Reset()
        {
            dependenciesRegistered = false;
        }

        public void RegisterDependencies()
        {
            Initialize(this.GetType());
        }

        public static string GetAssembliesPrefix(Type type)
        {
            string name = type.Assembly.GetName().Name;
            name = name.Substring(0, name.LastIndexOf("."));
            return name;
        }

        public void EnsureDependenciesRegistered()
        {
            if (!dependenciesRegistered)
            {
                lock (sync)
                {   
                    if (!dependenciesRegistered)
                    {
                        RegisterDependencies();
                        dependenciesRegistered = true;
                    }
                }
            }
        }

        public IEnumerable<string> GetInstanceNamesFor<TInterface>()
        {
            return ObjectFactory.Model.InstancesOf<TInterface>().Select(instance => instance.Name);
        }

        public object GetService(Type serviceType)
        {
            return ObjectFactory.GetInstance(serviceType);
        }

        public object GetInstance(Type serviceType)
        {
            return ObjectFactory.GetInstance(serviceType);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return key.IsEmpty() ? ObjectFactory.GetInstance(serviceType) : ObjectFactory.GetNamedInstance(serviceType, key);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return ObjectFactory.GetAllInstances(serviceType).Cast<object>();
        }

        public TService GetInstance<TService>()
        {
            return ObjectFactory.GetInstance<TService>();
        }

        public TService GetInstance<TService>(string key)
        {
            return (TService)ObjectFactory.GetNamedInstance(typeof(TService), key);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return (IEnumerable<TService>)ObjectFactory.GetAllInstances<TService>().AsQueryable();
        }
    }
}