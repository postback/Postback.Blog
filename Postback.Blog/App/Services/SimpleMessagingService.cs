using System.Collections.Generic;
using Postback.Blog.App.Messaging;

namespace Postback.Blog.App.Services
{
    public class SimpleMessagingService : IMessagingService
    {
         private readonly IHandlerFactory handlerFactory;

        public SimpleMessagingService(IHandlerFactory handerFactory)
        {
            this.handlerFactory = handerFactory;
        }

        public ExecutionResult Send(IMessage message)
        {
            ExecutionResult result = new ExecutionResult();
            IEnumerable<IHandler> handlers = handlerFactory.GetHandlers(message.GetType());

            foreach (IHandler handler in handlers)
            {
                ReturnValue returnObject = handler.Handle(message);
                result.Merge(returnObject);

                if (!result.Successful)
                {
                    break;
                }
            }

            return result;
        }
    }
}