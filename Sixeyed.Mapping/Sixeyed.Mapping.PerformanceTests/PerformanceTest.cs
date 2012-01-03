using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Tests.Stubs.Strategies;
using Sixeyed.Mapping.Tests.Stubs.Maps;
using System.Diagnostics;
using System.Reflection;
using System.Linq.Expressions;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.CachingStrategies;

namespace Sixeyed.Mapping.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PerformanceTest : TestBase
    {
        public PerformanceTest() { }

        public const int PerformanceIterations = 1000; //100000;

        [TestMethod]
        public void UserToUserModel_Manual()
        {
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = new UserModel();
                partial.Id = user.Id;
                partial.FirstName = user.FirstName;
                partial.LastName = user.LastName;
                partial.DateOfBirth = user.DateOfBirth;
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void UserToUserModel_AutoMap_ExactNaming()
        {
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = new AutoMap<User, UserModel>().Matching<ExactNameMatchingStrategy>().Create(user);
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void UserToUserModel_AutoMap_SimpleNaming()
        {
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = new AutoMap<User, UserModel>().Matching<SimpleNameMatchingStrategy>().Create(user);
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void UserToUserModel_AutoMap_Cached()
        {
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = new AutoMap<User, UserModel>().Cache<DictionaryCachingStrategy>().Create(user);
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void UserToUserModel_AutoMap()
        {
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = new AutoMap<User, UserModel>().Create(user);
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void UserToUserModel_AutoMap_Reused()
        {
            var map = new AutoMap<User, UserModel>();
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = map.Create(user);
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void UserToUserModel_StaticMap()
        {
            var map = new UserToUserModelMap();
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = map.Create(user);
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void ReflectionVersusExpression()
        {
            var user = GetFullUser();
            var firstName = user.FirstName;
            var getFirstName = (from p in typeof(User).GetProperties()
                                where p.Name == "FirstName"
                                select p).Single();
            var getFirstNameMethod = getFirstName.GetGetMethod();
            var reflectedFirstName = getFirstNameMethod.Invoke(user, null);
            Assert.AreEqual(firstName, reflectedFirstName);

            var dlg = GetGetMethodDelegate(getFirstNameMethod);
            var dlgFirstName = dlg(user);
            Assert.AreEqual(firstName, dlgFirstName);
        }

        static Func<object, object> GetDelegate(Type type, MethodInfo method)
        {
            var returnType = method.ReturnType;
            var instance = Expression.Parameter(type, "instance");
            var methodCall = Expression.Call(
                instance,
                method);

            return Expression.Lambda<Func<object, object>>(
               Expression.Convert(methodCall, returnType), instance).Compile();

        }

        [TestMethod]
        public void ExpressionDelegate()
        {
            var indexOf = typeof(string).GetMethod("IndexOf", new[] { typeof(char) });
            var getByteCount = typeof(Encoding).GetMethod("GetByteCount", new[] { typeof(string) });
            var indexOfFunc = MagicMethod<string>(indexOf);
            var getByteCountFunc = MagicMethod<Encoding>(getByteCount);
            var index = indexOfFunc("Hello", 'e');
            Assert.AreEqual(1, index);
            var byteCount = getByteCountFunc(Encoding.UTF8, "Euro sign: \u20ac");
            Assert.AreEqual(14, byteCount);
        }

        static Func<T, object, object> MagicMethod<T>(MethodInfo method)
        {
            var parameter = method.GetParameters().Single();
            var instance = Expression.Parameter(typeof(T), "instance");
            var argument = Expression.Parameter(typeof(object), "argument");
            var methodCall = Expression.Call(
                instance,
                method,
                Expression.Convert(argument, parameter.ParameterType)
                );

            return Expression.Lambda<Func<T, object, object>>(
                Expression.Convert(methodCall, typeof(object)),
                instance, argument
                ).Compile();
        }

        [TestMethod]
        public void ReflectionVersusOpenDelegate()
        {
            var user = GetFullUser();
            var firstName = user.FirstName;
            var getFirstName = (from p in typeof(User).GetProperties()
                                where p.Name == "FirstName"
                                select p).Single();
            var getFirstNameMethod = getFirstName.GetGetMethod();
            var reflectedFirstName = getFirstNameMethod.Invoke(user, null);
            Assert.AreEqual(firstName, reflectedFirstName);

            var dlg = MagicMethod0(getFirstNameMethod);
            var dlgFirstName = dlg(user);
            Assert.AreEqual(firstName, dlgFirstName);
        }

        static Func<object, object> MagicMethod0(MethodInfo method)
        {
            var type = method.DeclaringType;

            // First fetch the generic form
            MethodInfo genericHelper = typeof(PerformanceTest).GetMethod("MagicMethodHelper",
                BindingFlags.Static | BindingFlags.NonPublic);

            // Now supply the type arguments
            MethodInfo constructedHelper = genericHelper.MakeGenericMethod
                (type, method.ReturnType);

            // Now call it. The null argument is because it's a static method.
            object ret = constructedHelper.Invoke(null, new object[] { method });

            // Cast the result to the right kind of delegate and return it
            return (Func<object, object>)ret;
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

        [TestMethod]
        public void SetReflectionVersusOpenDelegate()
        {
            var user = GetFullUser();
            var firstName = user.FirstName;
            var getFirstName = (from p in typeof(User).GetProperties()
                                where p.Name == "FirstName"
                                select p).Single();
            var setFirstNameMethod = getFirstName.GetSetMethod();
            var newName = RandomGuidString();
            setFirstNameMethod.Invoke(user, new object[] {newName});
            Assert.AreEqual(newName, user.FirstName);

            newName = RandomGuidString();
            var dlg = MagicMethodSetter(setFirstNameMethod, getFirstName.PropertyType);
            dlg(user, newName);
            Assert.AreEqual(newName, user.FirstName);
        }

        static Action<object, object> MagicMethodSetter(MethodInfo method, Type propertyType)
        {
            var type = method.DeclaringType;

            // First fetch the generic form
            MethodInfo genericHelper = typeof(PerformanceTest).GetMethod("MagicMethodHelperSet",
                BindingFlags.Static | BindingFlags.NonPublic);

            // Now supply the type arguments
            MethodInfo constructedHelper = genericHelper.MakeGenericMethod
                (type, propertyType);

            // Now call it. The null argument is because it's a static method.
            object ret = constructedHelper.Invoke(null, new object[] { method });

            // Cast the result to the right kind of delegate and return it
            return (Action<object, object>)ret;
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

        static Func<object, object> GetGetMethodDelegate(MethodInfo method)
        {
            var type = method.DeclaringType;
            var instance = Expression.Parameter(typeof(object), "instance");
            var typedInstance = Expression.Convert(instance, type);
            var methodCall = Expression.Call(
                typedInstance,
                method
                );

            return Expression.Lambda<Func<object, object>>(
                Expression.Convert(methodCall, typeof(object)),
                instance).Compile();
        }


        private static User GetFullUser()
        {
            User user = new User
            {
                Id = RandomGuid(),
                FirstName = RandomGuidString(),
                LastName = RandomGuidString(),
                DateOfBirth = RandomDate(),
                EmailAddress = RandomGuidString(),
                NationalInsuranceNumber = RandomGuidString(),
                Address = new Address
                {
                    Line1 = RandomGuidString(),
                    Line2 = RandomGuidString(),
                    PostCode = new PostCode
                    {
                        InwardCode = RandomGuidString().Substring(0, 3),
                        OutwardCode = RandomGuidString().Substring(0, 3)
                    }
                }
            };
            return user;
        }

        private static void AssertEqual(User user, UserModel partial)
        {
            Assert.IsNotNull(partial);
            Assert.AreEqual(user.Id, partial.Id);
            Assert.AreEqual(user.FirstName, partial.FirstName);
            Assert.AreEqual(user.LastName, partial.LastName);
            Assert.AreEqual(user.DateOfBirth, partial.DateOfBirth);
            Assert.AreEqual(default(DateTime), partial.LastModified);
        }
    }
}
