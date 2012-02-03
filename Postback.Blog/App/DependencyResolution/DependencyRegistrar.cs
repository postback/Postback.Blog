using Postback.Blog.App.Bootstrap;
using StructureMap;

namespace Postback.Blog.App.DependencyResolution
{
    public class DependencyRegistrar
    {
        public static bool alreadyRegistered;

        public static void EnsureDependenciesRegistered()
        {
            if(!alreadyRegistered)
            {
                RegisterDependencies();
                alreadyRegistered = true;
            }
        }

        private static void RegisterDependencies()
        {
            ObjectFactory.Initialize(x => x.Scan(scan =>
                                                     {
                                                         scan.TheCallingAssembly();
                                                         scan.WithDefaultConventions();
                                                         scan.LookForRegistries();
                                                         scan.AddAllTypesOf<IStartUpTask>();
                                                     }));
        }
    }
}