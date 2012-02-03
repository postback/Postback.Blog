using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Postback.Blog.App.Services;

namespace Postback.Blog.App.Messaging
{
    public class ExecutionResult
    {
        private readonly List<ErrorMessage> messages = new List<ErrorMessage>();
        private readonly IDictionary<Type, object> returnItems = new Dictionary<Type, object>();

        public ExecutionResult() { }

        public ExecutionResult(IDictionary<Type, object> returnItems)
        {
            this.returnItems = returnItems;
        }

        public bool Successful
        {
            get { return messages.Count == 0; }
        }

        public IEnumerable<ErrorMessage> Messages
        {
            get { return messages; }
        }

        public IDictionary<Type, object> ReturnItems
        {
            get { return returnItems; }
        }

        public ExecutionResult AddMessage(string message, LambdaExpression attributeExpression, LambdaExpression uiExpression)
        {
            messages.Add(new ErrorMessage(message, uiExpression, attributeExpression));
            return this;
        }

        public ExecutionResult AddMessage(string message, LambdaExpression attributeExpression, LambdaExpression uiExpression, LambdaExpression compareToExpression)
        {
            messages.Add(new ErrorMessage(message, uiExpression, attributeExpression, compareToExpression));
            return this;
        }

        public void MergeWith(params ExecutionResult[] otherResults)
        {
            if (otherResults == null || otherResults.Length == 0) return;

            foreach (ExecutionResult otherResult in otherResults)
            {
                foreach (ErrorMessage message in otherResult.Messages)
                {
                    messages.Add(message);
                }

                foreach (KeyValuePair<Type, object> returnItem in otherResult.ReturnItems)
                {
                    returnItems.Add(returnItem.Key, returnItem.Value);
                }
            }
        }

        public void Merge(ReturnValue returnObject)
        {
            if (returnObject != null && returnObject.Type != null)
            {
                ReturnItems.Add(returnObject.Type, returnObject.Value);
            }
        }
    }
}