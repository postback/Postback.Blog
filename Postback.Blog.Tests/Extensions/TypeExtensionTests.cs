using System;
using NBehave.Spec.NUnit;
using NUnit.Framework;

namespace Postback.Blog.Tests.Extensions
{
    public interface IThing<T>
    {
    }

    [TestFixture]
    public class TypeExtensionsTests
    {
        public class ThingImpl1 : IThing<string> { }
        public class ThingImpl2 { }
        public class ThingImpl2<T> : IThing<T> { }
        public class ThingImpl3<T> { }
        public class ThingImpl3 : ThingImpl3<string> { }

        public class GenericThing<T> { }
        public class ClosedGenericThing : GenericThing<string> { }
        public interface ThingInterface : IThing<string> { }

        public class MyClass { }
        public interface IMySimpleInterface { }
        public enum MyEnum { }

        public interface IToImplement { }
        public abstract class AbstractImpl : IToImplement { }
        public class ImplAbstract : AbstractImpl { }
        public class ImplSuper : IToImplement { }
        public class ImplSubOfImpl : ImplSuper { }

        [Test]
        public void CanBeCastToFromNullFails()
        {
            Type type = null;
            type.CanBeCastTo<String>().ShouldBeFalse();
        }

        [Test]
        public void CanBeCastTo()
        {
            typeof(ImplSuper).CanBeCastTo<IToImplement>().ShouldBeTrue();
            typeof(ImplSubOfImpl).CanBeCastTo<IToImplement>().ShouldBeTrue();
            typeof(ImplSubOfImpl).CanBeCastTo<ImplSuper>().ShouldBeTrue();

            typeof(ImplSuper).CanBeCastTo<ImplSuper>().ShouldBeTrue();
            typeof(ImplSuper).CanBeCastTo<ImplSubOfImpl>().ShouldBeFalse();
            typeof(ClosedGenericThing).CanBeCastTo<string>().ShouldBeFalse();
        }


    }
}
