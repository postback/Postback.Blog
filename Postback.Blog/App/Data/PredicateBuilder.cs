using System;
using System.Linq.Expressions;

namespace Postback.Blog.App.Data
{
    /// <summary>
    /// By Jb Evain, found at http://evain.net/blog/articles/2010/07/20/predicatebuilder-revisited
    /// </summary>
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>()
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Constant(true), Expression.Parameter(typeof(T)));
        }

        public static Expression<Func<T, bool>> False<T>()
        {
            return Expression.Lambda<Func<T, bool>>(Expression.Constant(false), Expression.Parameter(typeof(T)));
        }

        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> self, Expression<Func<T, bool>> expression)
        {
            return Combine(self, expression, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> self, Expression<Func<T, bool>> expression)
        {
            return Combine(self, expression, Expression.AndAlso);
        }

        static Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>> self, Expression<Func<T, bool>> expression, Func<Expression, Expression, Expression> selector)
        {
            CheckSelfAndExpression(self, expression);

            var parameter = CreateParameterFrom(self);

            return Expression.Lambda<Func<T, bool>>(
                selector(
                    RewriteLambdaBody(self, parameter),
                    RewriteLambdaBody(expression, parameter)),
                parameter);
        }

        static Expression RewriteLambdaBody(LambdaExpression expression, ParameterExpression parameter)
        {
            return new ParameterRewriter(expression.Parameters[0], parameter).Visit(expression.Body);
        }

        class ParameterRewriter : ExpressionVisitor
        {

            readonly ParameterExpression candidate;
            readonly ParameterExpression replacement;

            public ParameterRewriter(ParameterExpression candidate, ParameterExpression replacement)
            {
                this.candidate = candidate;
                this.replacement = replacement;
            }

            protected override Expression VisitParameter(ParameterExpression expression)
            {
                return ReferenceEquals(expression, candidate) ? replacement : expression;
            }
        }

        static ParameterExpression CreateParameterFrom<T>(Expression<Func<T, bool>> left)
        {
            var template = left.Parameters[0];

            return Expression.Parameter(template.Type, template.Name);
        }

        static void CheckSelfAndExpression<T>(Expression<Func<T, bool>> self, Expression<Func<T, bool>> expression)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (expression == null)
                throw new ArgumentNullException("expression");
        }
    } 
}