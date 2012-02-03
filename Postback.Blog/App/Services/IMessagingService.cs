using Postback.Blog.App.Messaging;

namespace Postback.Blog.App.Services
{
    public interface IMessagingService
    {
        ExecutionResult Send(IMessage message);
    }

    public interface IMessage
    {}
}