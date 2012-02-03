using System.Dynamic;
using Postback.Blog.App.Data;
using Postback.Blog.App.Services;

namespace Postback.Blog.App.Messaging
{
    public class MailNewPasswordHandler : Handler<NewPasswordMessage>
    {
        private IPersistenceSession session;

        public MailNewPasswordHandler(IPersistenceSession session)
        {
            this.session = session;
        }

        protected override ReturnValue Handle(NewPasswordMessage message)
        {
            session.InsertIntoCollection(new { Type= "email", message.User, message.NewPassword  }, "outgoing");

            return null;
        }
    }
}