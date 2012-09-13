using System.Security.Principal;

namespace Postback.Blog.App.Security
{
    public interface ISecurityContext
    {
        IPrincipal CurrentUser { get; }
        bool IsAuthenticated();
    }
}