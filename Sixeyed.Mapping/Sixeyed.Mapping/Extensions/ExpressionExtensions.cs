using System.Linq.Expressions;
using System.Reflection;

namespace Sixeyed.Mapping.Extensions
{
    /// <summary>
    /// Extensions to <see cref="Expression"/>
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Navigates an expression returning the nearest <see cref="ParameterExpression"/> in the tree
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static ParameterExpression GetParameterExpression(this Expression expression)
        {
            ParameterExpression parameterExpression = null;
            while (expression.NodeType == ExpressionType.MemberAccess)
            {
                expression = ((MemberExpression)expression).Expression;
            }
            if (expression.NodeType == ExpressionType.Parameter)
            {
                parameterExpression = (ParameterExpression)expression;
            }
            return parameterExpression;
        }
    }
}
