using System;
using System.Collections.Generic;

namespace DuoCode.SimpleInjector.InvokeStrategies
{
    internal class ParameterInvoker : IInvokeStrategy
    {
        private readonly Type type;
        private Func<Type, object> invoker;
        private static readonly Type enumerableType = typeof(IEnumerable<>);

        public ParameterInvoker(Type parameterType, IContainer container)
        {
            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition().IsAssignableFrom(enumerableType))
            {
                type = parameterType.GetGenericArguments()[0];
                invoker = container.GetAll;
            }
            else
            {
                type = parameterType;
                invoker = container.Get;
            }
        }

        public object Get()
        {
            return invoker(type);
        }
    }
}