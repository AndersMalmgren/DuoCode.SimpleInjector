using System;
using System.Collections.Generic;
using System.Linq;

namespace DuoCode.SimpleInjector.InvokeStrategies
{
    internal class TypeInvokeStrategy : IInvokeStategy
    {
        private readonly Type to;
        private readonly IContainer container;
        private static readonly Type enumerableType = typeof(IEnumerable<>);
        
        public TypeInvokeStrategy(Type to, IContainer container)
        {
            this.to = to;
            this.container = container;
        }
        
        public object Get()
        {
            if (to.IsInterface || to.IsAbstract) throw new Exception("Can only inject concrete none abstract types");

            var constructor = to.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            var parameters = constructor.GetParameters()
                .Select(p => p.ParameterType.IsGenericType && p.ParameterType.GetGenericTypeDefinition().IsAssignableFrom(enumerableType) ? container.GetAll(p.ParameterType.GetGenericArguments()[0]) : container.Get(p.ParameterType))
                .ToArray();

            var instance = constructor.Invoke(parameters);
            return instance;
        }
    }
}
