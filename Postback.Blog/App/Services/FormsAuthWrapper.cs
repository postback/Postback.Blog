using System.Web.Security;

namespace Postback.Blog.App.Services
{
    public class FormsAuthWrapper : IAuth
    {
        public void DoAuth(string username, bool remember)
        {
            FormsAuthentication.SetAuthCookie(username, remember);
        }
    }
}