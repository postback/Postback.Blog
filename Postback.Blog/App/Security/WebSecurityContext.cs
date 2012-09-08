using System.Security.Principal;
using System.Web;
using Microsoft.Practices.ServiceLocation;

namespace Postback.Blog.App.Security
{
    public class WebSecurityContext : ISecurityContext
    {
        private readonly HttpContextBase httpContext;

        public WebSecurityContext()
        {
            httpContext = ServiceLocator.Current.GetInstance<HttpContextBase>();
        }

        public bool IsAuthenticated()
        {
            return httpContext.Request.IsAuthenticated;
        }

        public IPrincipal CurrentUser
        {
            get
            {
                return httpContext.User;
            }
        }
    }
}