using System;

namespace Postback.Blog.App.Services
{
    public abstract class Handler<T> : IHandler
    {
        public ReturnValue Handle(object commandMessage)
        {
            return Handle((T)commandMessage);
        }

        protected abstract ReturnValue Handle(T commandMessage);
    }

    public interface IHandler : IHandler<object, ReturnValue>
    {

    }

    public interface IHandler<TCommand, TResult>
    {
        TResult Handle(TCommand commandMessage);
    }

    public class ReturnValue
    {
        public Type Type { get; set; }
        public object Value { get; set; }

        public ReturnValue SetValue<T>(T input)
        {
            Type = typeof(T);
            Value = input;
            return this;
        }
    }
}