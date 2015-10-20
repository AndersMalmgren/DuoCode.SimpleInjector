using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DuoCode.Runtime;
using DuoCode.SimpleInjector.InvokeStrategies;

namespace DuoCode.SimpleInjector
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, List<IInvokeStrategy>> bindings = new Dictionary<Type, List<IInvokeStrategy>>();

        public Container()
        {
            Bind<IContainer>(this);
        }

        public BindingResult Bind<TSource, TTo>() where TTo : class, TSource
        {
            return AddBinding(typeof(TSource), new TypeInvokeStrategy(typeof(TTo), this));
        }

        public BindingResult Bind<TSource>(TSource constant) where TSource : class
        {
            return AddBinding(typeof(TSource), new ConstantStrategy<TSource>(constant));
        }

        public BindingResult Bind(Type source, Type to)
        {
            return AddBinding(source, new TypeInvokeStrategy(to, this));
        }

        public T Get<T>() where T : class
        {
            return Get(typeof(T)) as T;
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return GetAll(typeof (T)).Cast<T>();
        }

        public IEnumerable GetAll(Type type)
        {
            var invokers = GetBindings(type);

            if(invokers == null)
                return new[] { new TypeInvokeStrategy(type, this).Get(type) };

            return invokers.Select(b => b.Get(type));
        }

        public object Get(Type type)
        {
            var invokers = GetBindings(type);

            if(invokers == null)
                return new TypeInvokeStrategy(type, this).Get(type);

            if(invokers.Count > 1) throw new Exception(string.Format("Multiple instances registered for type: {0}", type.FullName));

            return invokers[0].Get(type);
        }

        private List<IInvokeStrategy> GetBindings(Type type)
        {
            if (bindings.ContainsKey(type)) return bindings[type];
            if(OpenGenericTypes.IsUnknownOpenType(type))
            {
                var openType = type.GetGenericTypeDefinition();
                if (bindings.ContainsKey(openType)) return bindings[openType];
            }

            return null;
        }

        private BindingResult AddBinding(Type typeSource, IInvokeStrategy invoker)
        {
            if(!bindings.ContainsKey(typeSource))
                bindings[typeSource] = new List<IInvokeStrategy>();

            bindings[typeSource].Add(invoker);

            return new BindingResult(invoker, newInvoker =>
            {
                bindings[typeSource].Remove(invoker);
                bindings[typeSource].Add(newInvoker);
            });
        }
    }
}
