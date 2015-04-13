using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DuoCode.SimpleInjector.InvokeStrategies
{
    internal class TypeInvokeStrategy : IInvokeStrategy
    {
        private readonly Type to;
        private readonly IContainer container;
        private static readonly Type enumerableType = typeof(IEnumerable<>);
        private ConstructorInfo constructor;
        private object[] parameters;

        public TypeInvokeStrategy(Type to, IContainer container)
        {
            this.to = to;
            this.container = container;

            if (to.IsInterface || to.IsAbstract) throw new Exception("Can only inject concrete none abstract types");

            constructor = to.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            parameters = constructor.GetParameters()
                .Select(p => p.ParameterType.IsGenericType && p.ParameterType.GetGenericTypeDefinition().IsAssignableFrom(enumerableType) ? container.GetAll(p.ParameterType.GetGenericArguments()[0]) : container.Get(p.ParameterType))
                .ToArray();
        }
        
        public object Get()
        {
            var instance = constructor.Invoke(parameters);
            return instance;
        }
    }
}
