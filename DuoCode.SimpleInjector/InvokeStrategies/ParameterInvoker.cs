using System;
using System.Collections.Generic;

namespace DuoCode.SimpleInjector.InvokeStrategies
{
    internal class ParameterInvoker : IInvokeStrategy
    {
        private readonly Type type;
        private Func<Type, object> invoker;
        private static readonly Type enumerableType = typeof(IEnumerable<>);
        private static readonly Type funcType = typeof(Func<>);

        public ParameterInvoker(Type parameterType, IContainer container)
        {
            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition().IsAssignableFrom(enumerableType))
            {
                type = parameterType.GetGenericArguments()[0];
                invoker = container.GetAll;
            }
            else if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition().IsAssignableFrom(funcType))
            {
                type = parameterType.GetGenericArguments()[0];
                var parameterInvoker = new ParameterInvoker(type, container);
                invoker = t => new Func<object>(parameterInvoker.Get); //Func<object> only works because the CLR is javascript;
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