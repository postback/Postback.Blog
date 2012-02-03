using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Postback.Blog.App.DependencyResolution;
using Postback.Blog.Areas.Admin.Controllers;

namespace Postback.Blog.Tests.DI
{
    [TestFixture]
    public class DependencyRegistrarTester
    {

        private IEnumerable<Type> GetControllers()
        {
            Type[] types = typeof(DashboardController).Assembly.GetTypes();
            return types.Where(e => IsAController(e));
        }

        private bool IsAController(Type e)
        {
            return e.GetInterface("IController") != null && !e.IsAbstract;
        }

    }
}
