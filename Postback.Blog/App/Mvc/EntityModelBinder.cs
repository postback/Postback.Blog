using System;
using System.Web.Mvc;
using Postback.Blog.App.Data;
using Postback.Blog.Models;
using Raven.Client;

namespace Postback.Blog.App.Mvc
{
    public class EntityModelBinder<TEntity> : DefaultModelBinder where TEntity : Entity,new ()
    {
        readonly IDocumentSession session;

        public EntityModelBinder(IDocumentSession session)
        {
            this.session = session;
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType.CanBeCastTo<Entity>())
            {
                var id = GetModelId(bindingContext);
                if (!string.IsNullOrEmpty(id))
                {
                    return session.Load<TEntity>(id);
                }
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            var entity = bindingContext.ModelMetadata.Model as Entity;
            if (entity != null)
            {
                var id = GetModelId(bindingContext);
                if (!string.IsNullOrEmpty(id) && id != entity.Id)
                {
                    bindingContext.ModelMetadata.Model = null;
                }
            }
            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }


        private static string GetModelId(ModelBindingContext bindingContext)
        {
            var fullPropertyKey = CreateSubPropertyName(bindingContext.ModelName, "Id");
            if (!bindingContext.ValueProvider.ContainsPrefix(fullPropertyKey))
            {
                return null;
            }

            var result = bindingContext.ValueProvider.GetValue(fullPropertyKey);
            if (result == null)
            {
                return null;
            }

            var idAsObject = result.ConvertTo(typeof(String));
            if (idAsObject == null)
            {
                return null;
            }

            return idAsObject as String;
        }

        protected virtual void ValidateEntity(ModelBindingContext bindingContext, ControllerContext controllerContext, object entity)
        {
        }

        // For unit tests
        public void SetModelBinderDictionary(ModelBinderDictionary modelBinderDictionary)
        {
            Binders = modelBinderDictionary;
        }
    }
}