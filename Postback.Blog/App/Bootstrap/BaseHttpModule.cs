using System.Web;

namespace Postback.Blog.App.Bootstrap
{
    /// <summary>
    /// Inheriting from this Base IHttpModule, allows us to unit test the implementing module
    /// </summary>
    public abstract class BaseHttpModule : IHttpModule
    {
        public void Init(HttpApplication app)
        {
            app.BeginRequest += (sender, e) => OnBeginRequest(new HttpContextWrapper(((HttpApplication)sender).Context));
            app.Error += (sender, e) => OnError(new HttpContextWrapper(((HttpApplication)sender).Context));
            app.EndRequest += (sender, e) => OnEndRequest(new HttpContextWrapper(((HttpApplication)sender).Context));
            app.PostAuthenticateRequest += (sender, e) => OnPostAuthenticateRequest(new HttpContextWrapper(((HttpApplication)sender).Context));
        }

        public void Dispose()
        {
        }

        public virtual void OnBeginRequest(HttpContextBase context)
        {
        }

        public virtual void OnError(HttpContextBase context)
        {
        }

        public virtual void OnEndRequest(HttpContextBase context)
        {
        }

        public virtual void OnPostAuthenticateRequest(HttpContextBase context)
        {
        }
    }
}