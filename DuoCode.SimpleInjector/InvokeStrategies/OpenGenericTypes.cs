using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoCode.SimpleInjector.InvokeStrategies
{
    internal static class OpenGenericTypes
    {
        private static readonly Type enumerableType = typeof(IEnumerable<>);
        private static readonly Type funcType = typeof(Func<>);

        public static bool IsEnumerable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(enumerableType);
        }

        public static bool IsFunc(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(funcType);
        }

        public static bool IsUnknownOpenType(Type type)
        {
            if (!type.IsGenericType) return false;

            return !IsEnumerable(type) && !IsFunc(type);
        }
    }
}
