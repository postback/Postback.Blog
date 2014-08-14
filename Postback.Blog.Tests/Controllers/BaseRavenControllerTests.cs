using Postback.Blog.Tests.Data;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Listeners;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Postback.Blog.Tests.Controllers
{
    public abstract class BaseRavenControllerTests : BaseRavenTest
	{
		protected ControllerContext ControllerContext { get; set; }

		protected void SetupData(Action<IDocumentSession> action)
		{
			using (var session = store.OpenSession())
			{
				action(session);
				session.SaveChanges();
			}
		}

		protected void ExecuteAction<TController>(Action<TController> action) 
			where TController: Controller, new()
		{
			var controller = new TController {};

			var httpContext = MockRepository.GenerateStub<HttpContextBase>();
			httpContext.Stub(x => x.Response).Return(MockRepository.GenerateStub<HttpResponseBase>());
			ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);
			controller.ControllerContext = ControllerContext;

			action(controller);

			//controller.RavenSession.SaveChanges();
		}
	}
}
