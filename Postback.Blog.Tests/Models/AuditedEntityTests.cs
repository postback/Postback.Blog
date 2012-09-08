using System;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using Postback.Blog.Models;

namespace Postback.Blog.Tests.Models
{
    [TestFixture]
    public class AuditedEntityTests
    {
        [Test]
        public void CanCreateAndSetAuditedEntity()
        {
            SystemTime.Now = () => new DateTime(9,8,7,6,5,4);
            var a = new DummyEntity {CreatedBy= "vincent",Created = SystemTime.Now()};
            a.ShouldNotBeNull();
            a.Created.ShouldNotBeNull();
            a.CreatedBy.ShouldNotBeNull();
            a.CreatedBy.ShouldEqual("vincent");
            a.Created.ShouldEqual(SystemTime.Now());
        }
    }

    public class DummyEntity : AuditedEntity
    {

    }
}
