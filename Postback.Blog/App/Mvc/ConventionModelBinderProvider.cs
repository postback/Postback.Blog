using System;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Postback.Blog.Models;

namespace Postback.Blog.App.Mvc
{
    public class ConventionModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType.CanBeCastTo<Entity>())
            {
                var entityBinderType = typeof(EntityModelBinder<>).MakeGenericType(modelType);
                var entityBinder = ServiceLocator.Current.GetInstance(entityBinderType);
                return (IModelBinder)entityBinder;
            }

            return null;
        }
    }
}