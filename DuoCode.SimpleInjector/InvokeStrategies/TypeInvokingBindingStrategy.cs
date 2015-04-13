using System;
using System.Linq;
using System.Reflection;

namespace DuoCode.SimpleInjector.InvokeStrategies
{
    internal class TypeInvokeStrategy : IInvokeStrategy
    {
        private readonly Type to;
        private readonly IContainer container;
        private ConstructorInfo constructor;
        private IInvokeStrategy[] parameterInvokers;

        public TypeInvokeStrategy(Type to, IContainer container)
        {
            this.to = to;
            this.container = container;

            if (to.IsInterface || to.IsAbstract) throw new Exception("Can only inject concrete none abstract types");

            constructor = to.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            parameterInvokers = constructor.GetParameters()
                .Select(p => new ParemeterInvoker(p.ParameterType, container))
                .ToArray();
        }
        
        public object Get()
        {
            var instance = constructor.Invoke(parameterInvokers.Select(p => p.Get()).ToArray());
            return instance;
        }
    }
}
