using System.Dynamic;
using Postback.Blog.App.Data;
using Postback.Blog.App.Services;
using Raven.Client;

namespace Postback.Blog.App.Messaging
{
    public class MailNewPasswordHandler : Handler<NewPasswordMessage>
    {
        private IDocumentSession session;

        public MailNewPasswordHandler(IDocumentSession session)
        {
            this.session = session;
        }

        protected override ReturnValue Handle(NewPasswordMessage message)
        {
            //session.Add(new { Type= "email", message.User, message.NewPassword  });

            return null;
        }
    }
}