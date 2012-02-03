using Postback.Blog.App.Services;
using Postback.Blog.Models;

namespace Postback.Blog.App.Messaging
{
    public class NewPasswordMessage : IMessage
    {
        public User User { get; set; }
        public string NewPassword { get; set; }
    }
}