using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Postback.Blog.App.Messaging
{
    public class ErrorMessage
    {
        public ErrorMessage(string messageText, LambdaExpression uiAttribute, LambdaExpression incorrectAttribute,
                            LambdaExpression compareToAttribute)
        {
            UIAttribute = uiAttribute;
            IncorrectAttribute = incorrectAttribute;
            ComparedAttribute = compareToAttribute;
            MessageText = messageText;
        }

        public ErrorMessage(string messageText, LambdaExpression uiAttribute, LambdaExpression incorrectAttribute)
            : this(messageText, uiAttribute, incorrectAttribute, null) { }

        public LambdaExpression IncorrectAttribute { get; private set; }
        public LambdaExpression ComparedAttribute { get; private set; }
        public LambdaExpression UIAttribute { get; private set; }
        public string MessageText { get; private set; }
    }
}