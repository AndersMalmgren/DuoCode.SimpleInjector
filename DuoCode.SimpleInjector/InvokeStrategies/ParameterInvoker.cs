using System;
using System.Collections.Generic;
using System.Linq;

namespace DuoCode.SimpleInjector.InvokeStrategies
{
    internal class ParameterInvoker : IInvokeStrategy
    {
        private readonly Type type;
        private Func<Type, object> invoker;

        public ParameterInvoker(Type parameterType, IContainer container)
        {
            if (OpenGenericTypes.IsEnumerable(parameterType))
            {
                type = parameterType.GetGenericArguments()[0];
                invoker = container.GetAll;
            }
            else if(OpenGenericTypes.IsFunc(parameterType))
            {
                type = parameterType.GetGenericArguments()[0];
                var parameterInvoker = new ParameterInvoker(type, container);
                invoker = t => new Func<object>(() => parameterInvoker.Get(t)); //Func<object> only works because the CLR is javascript;
            }
            else
            {
                type = parameterType;
                invoker = container.Get;
            }
        }

        public object Get(Type requestedType)
        {
            return invoker(type);
        }
    }
}