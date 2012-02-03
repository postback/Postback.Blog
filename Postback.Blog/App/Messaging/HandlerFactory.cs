using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Postback.Blog.App.Services;
using StructureMap;

namespace Postback.Blog.App.Messaging
{
    /// <summary>
    /// Gets all commands for a specific <see cref="Type">commandMessageType</see>
    /// </summary>
    public interface IHandlerFactory
    {
        IEnumerable<IHandler> GetHandlers(Type commandMessageType);
    }

    /// <summary>
    /// Implementation of <see cref="IHandlerFactory"/>. Gets all commands for a specific <see cref="Type">commandMessageType</see>
    /// </summary>
    public class HandlerFactory : IHandlerFactory
    {
        private static readonly Type genericHandler = typeof(Handler<>);

        public IEnumerable<IHandler> GetHandlers(Type commandMessageType)
        {
            Type concreteCommandType = genericHandler.MakeGenericType(new Type[] { commandMessageType });
            var commandImpl = ObjectFactory.GetAllInstances(concreteCommandType); ;
            var commands = commandImpl.Cast<IHandler>();
            return commands;
        }
    }
}