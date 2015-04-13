using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DuoCode.Runtime;

namespace DuoCode.SimpleInjector
{
    public class Container : IContainer
    {
        static Container()
        {
            //Temp hack until fixed by DuoCode team
            //http://stackoverflow.com/questions/1606797/use-of-apply-with-new-operator-is-this-possible

            Js.referenceAs<Action>(@"(function() {
    function construct(constructor, args) {
        function F() {
            return constructor.apply(this, args);
        }
        F.prototype = constructor.prototype;
        return new F();
    }

    System.Reflection.ConstructorInfo.prototype.Invoke$3 = function(invokeAttr, binder, parameters, culture) {
            return construct(this.jsConstructor, parameters);
        };
})")();
        }

        private static readonly Type enumerableType = typeof (IEnumerable<>);
        private readonly Dictionary<Type, List<Type>> bindings = new Dictionary<Type, List<Type>>();

        public void Bind<TSource, TTo>() where TTo : class, TSource
        {
            var typeSource = typeof (TSource);

            if (!bindings.ContainsKey(typeSource))
                bindings[typeSource] = new List<Type>();

            bindings[typeSource].Add(typeof(TTo));
        }

        public T Get<T>() where T : class
        {
            return Get(typeof(T)) as T;
        }

        public IEnumerable GetAll(Type type)
        {
            var  targetTypes = bindings.ContainsKey(type) ? bindings[type] as IEnumerable<Type> : new[] { type };

            var errors = targetTypes
                .Where(t => !(t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableFrom(enumerableType)) && (t.IsInterface || t.IsAbstract))
                .Select(t => string.Format("{0}: Can only inject concrete none abstract types", t.FullName));

            if (errors.Any())
                throw new Exception(string.Join(Environment.NewLine, errors));

            return targetTypes.Select(Invoke);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return GetAll(typeof (T)).Cast<T>();
        }

        private object Invoke(Type type)
        {
            var constructor = type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            var parameters = constructor.GetParameters()
                .Select(p => p.ParameterType.IsGenericType && p.ParameterType.GetGenericTypeDefinition().IsAssignableFrom(enumerableType) ? GetAll(p.ParameterType.GetGenericArguments()[0]) : Get(p.ParameterType))
                .ToArray();

            var instance = constructor.Invoke(parameters);
            return instance;
        }

        public object Get(Type type)
        {
            return GetAll(type).Cast<object>().Single();
        }
    }
}
