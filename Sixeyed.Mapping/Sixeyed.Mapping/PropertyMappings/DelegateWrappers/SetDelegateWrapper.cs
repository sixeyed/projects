using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Sixeyed.Mapping.PropertyMappings.DelegateWrappers
{
    /// <summary>
    /// Reflection optimization for setting property values
    /// </summary>
    /// <remarks>
    /// Lifted wholesale ftom Jon Skeet's blog post:
    /// http://msmvps.com/blogs/jon_skeet/archive/2008/08/09/making-reflection-fly-and-exploring-delegates.aspx
    /// </remarks>
    public static class SetDelegateWrapper
    {
        private static Dictionary<PropertyInfo, Action<object, object>> SetDelegates { get; set; }
        private static Dictionary<string, MethodInfo> SetMethods { get; set; }

        static SetDelegateWrapper()
        {
            SetMethods = new Dictionary<string, MethodInfo>();
            SetDelegates = new Dictionary<PropertyInfo, Action<object, object>>();
        }

        public static void SetValue(object target, object sourceValue, PropertyInfo setter)
        {
            if (!SetDelegates.ContainsKey(setter))
            {
                SetDelegates[setter] = MagicMethodSetter(setter.GetSetMethod(), setter.PropertyType);
            }
            SetDelegates[setter](target, sourceValue);
        }

        static Action<object, object> MagicMethodSetter(MethodInfo method, Type propertyType)
        {
            MethodInfo constructedHelper = GetMethodInfo(method.DeclaringType, propertyType);

            // Now call it. The null argument is because it's a static method.
            object ret = constructedHelper.Invoke(null, new object[] { method });

            // Cast the result to the right kind of delegate and return it
            return (Action<object, object>)ret;
        }

        private static MethodInfo GetMethodInfo(Type sourceType, Type propertyType)
        {
            var key = string.Format("{0}:{1}", sourceType.FullName, propertyType.FullName);
            if (!SetMethods.ContainsKey(key))
            {
                // First fetch the generic form
                MethodInfo genericHelper = typeof(SetDelegateWrapper).GetMethod("MagicMethodHelperSet",
                    BindingFlags.Static | BindingFlags.NonPublic);

                // Now supply the type arguments
                MethodInfo constructedHelper = genericHelper.MakeGenericMethod
                    (sourceType, propertyType);
                SetMethods[key] = constructedHelper;
            }
            return SetMethods[key];
        }

        static Action<object, object> MagicMethodHelperSet<TTarget, TParam>(MethodInfo method)
            where TTarget : class
        {
            // Convert the slow MethodInfo into a fast, strongly typed, open delegate
            Action<TTarget, TParam> func = (Action<TTarget, TParam>)Delegate.CreateDelegate
                (typeof(Action<TTarget, TParam>), method);

            // Now create a more weakly typed delegate which will call the strongly typed one
            Action<object, object> ret = (object target, object value) => func((TTarget)target, (TParam)value);
            return ret;
        }
    }
}
