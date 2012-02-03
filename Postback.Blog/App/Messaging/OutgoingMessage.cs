using System.Dynamic;
using Postback.Blog.Models;

namespace Postback.Blog.App.Messaging
{
    public class OutgoingMessage : Entity
    {
        public ExpandoObject Message { get; set; }
    }
}