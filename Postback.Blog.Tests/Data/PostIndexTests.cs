using NUnit.Framework;
using Postback.Blog.App.Data.Indexes;
using Postback.Blog.Models;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBehave.Spec.NUnit;

namespace Postback.Blog.Tests.Data
{
    [TestFixture]
    public class PostIndexTests : BaseRavenTest
    {
        private EmbeddableDocumentStore Store { get; set; }

        protected override void CreateDefaultIndexes(IDocumentStore documentStore)
        {
            base.CreateDefaultIndexes(documentStore);
            new PostSearchIndex().Execute(documentStore);
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            Store = NewStore();

            var asseta = new Post { Title = "Yeah", Body = "some content", Tags = new List<Tag> { new Tag("art") } };
            var assetb = new Post { Title = "Okay", Body = "okay yeah that's right", Tags = new List<Tag> { new Tag("art") } };
            var assetc = new Post { Title = "Trees", Body = "trees", Tags = new List<Tag> { new Tag("yeah") } };

            using (var session = Store.OpenSession())
            {
                session.Store(asseta);
                session.Store(assetb);
                session.Store(assetc);
                session.SaveChanges();
            }
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Store.Dispose();
        }

        [Test]
        public void ReturnsByKey()
        {
            using (var session = Store.OpenSession())
            {
                //Act
                var posts = session.Query<PostSearchIndex.Result, PostSearchIndex>().Search(x => x.Content, "yeah").Customize(a => a.WaitForNonStaleResults()).As<Post>().ToList();

                //Assert
                posts.ShouldNotBeNull();
                posts.Count.ShouldEqual(3);
            }
        } 
    }
}
