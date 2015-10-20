using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DuoCode.SimpleInjector.InvokeStrategies
{
    internal class TypeInvokeStrategy : IInvokeStrategy
    {
        private readonly Type to;
        private readonly IContainer container;
        private readonly IDictionary<Type, InvokeInfo> constructorCache = new Dictionary<Type, InvokeInfo>();

        public TypeInvokeStrategy(Type to, IContainer container)
        {
            this.to = to;
            this.container = container;

            if (to.IsInterface || to.IsAbstract) throw new Exception(string.Format("{0}: Can only inject concrete none abstract types", to.FullName));

            if(!to.IsGenericTypeDefinition)
                constructorCache[to] = CreateInvokeInfo(to);
        }

        public object Get(Type requestedType)
        {
            var ctrInfo = GetInvokeInfo(to.IsGenericTypeDefinition ? requestedType : to);

            var instance = ctrInfo.Constructor.Invoke(ctrInfo.ParameterInvokers.Select(p => p.Get(null)).ToArray());
            return instance;
        }

        private InvokeInfo CreateInvokeInfo(Type type)
        {
            if (type != to)
                type = to.MakeGenericType(type.GetGenericArguments());

            var ctr = type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            return new InvokeInfo
            {
                Constructor = ctr,
                ParameterInvokers = ctr.GetParameters()
                    .Select(p => new ParameterInvoker(p.ParameterType, container))
                    .ToArray()
            };
        }

        private InvokeInfo GetInvokeInfo(Type type)
        {
            if (!constructorCache.ContainsKey(type))
                constructorCache[type] = CreateInvokeInfo(type);

            return constructorCache[type];
        }


        private class InvokeInfo
        {
            public ConstructorInfo Constructor { get; set; }
            public ParameterInvoker[] ParameterInvokers { get; set; }
        }
    }
}
