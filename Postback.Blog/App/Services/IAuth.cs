namespace Postback.Blog.App.Services
{
    public interface IAuth
    {
        void DoAuth(string username, bool remember);
    }
}