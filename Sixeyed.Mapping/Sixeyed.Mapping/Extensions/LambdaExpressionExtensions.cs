using System.Linq.Expressions;
using System.Reflection;

namespace Sixeyed.Mapping.Extensions
{
    /// <summary>
    /// Extensions to <see cref="LambdaExpression"/>
    /// </summary>
    public static class LambdaExpressionExtensions
    {
        /// <summary>
        /// Returns the <see cref="PropertyInfo"/> referenced by the expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo AsPropertyInfo(this LambdaExpression expression)
        {
            PropertyInfo info = null;
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }
            if (memberExpression != null)
            {
                info = memberExpression.Member as PropertyInfo;
            }
            return info;
        }

    }
}
