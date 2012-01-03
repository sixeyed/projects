using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Sixeyed.Mapping.PropertyMappings.DelegateWrappers
{
    /// <summary>
    /// Reflection optimization for getting property values
    /// </summary>
    /// <remarks>
    /// Lifted wholesale ftom Jon Skeet's blog post:
    /// http://msmvps.com/blogs/jon_skeet/archive/2008/08/09/making-reflection-fly-and-exploring-delegates.aspx
    /// </remarks>
    public static class GetDelegateWrapper
    {
        private static Dictionary<PropertyInfo, Func<object, object>> GetDelegates { get; set; }
        private static Dictionary<string, MethodInfo> GetMethods { get; set; }

        static GetDelegateWrapper()
        {
            GetMethods = new Dictionary<string, MethodInfo>();
            GetDelegates = new Dictionary<PropertyInfo, Func<object, object>>();
        }

        public static object GetValue(object source, PropertyInfo getter)
        {
            object value = null;
            if (!GetDelegates.ContainsKey(getter))
            {
                GetDelegates[getter] = MagicMethod0(getter.GetGetMethod());
            }
            value = GetDelegates[getter](source);
            return value;
        }

        static Func<object, object> MagicMethod0(MethodInfo method)
        {
            MethodInfo constructedHelper = GetMethodInfo(method.DeclaringType, method.ReturnType);

            // Now call it. The null argument is because it's a static method.
            object ret = constructedHelper.Invoke(null, new object[] { method });

            // Cast the result to the right kind of delegate and return it
            return (Func<object, object>)ret;
        }

        private static MethodInfo GetMethodInfo(Type sourceType, Type propertyType)
        {
            var key = string.Format("{0}:{1}", sourceType.FullName, propertyType.FullName);
            if (!GetMethods.ContainsKey(key))
            {
                // First fetch the generic form
                MethodInfo genericHelper = typeof(GetDelegateWrapper).GetMethod("MagicMethodHelper",
                    BindingFlags.Static | BindingFlags.NonPublic);

                // Now supply the type arguments
                MethodInfo constructedHelper = genericHelper.MakeGenericMethod
                    (sourceType, propertyType);
                GetMethods[key] = constructedHelper;
            }
            return GetMethods[key];
        }

        static Func<object, object> MagicMethodHelper<TTarget, TReturn>(MethodInfo method)
            where TTarget : class
        {
            // Convert the slow MethodInfo into a fast, strongly typed, open delegate
            Func<TTarget, TReturn> func = (Func<TTarget, TReturn>)Delegate.CreateDelegate
                (typeof(Func<TTarget, TReturn>), method);

            // Now create a more weakly typed delegate which will call the strongly typed one
            Func<object, object> ret = (object target) => func((TTarget)target);
            return ret;
        }
    }
}
