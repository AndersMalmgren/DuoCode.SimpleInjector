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

        private readonly Dictionary<Type, List<IInvokeStategy>> bindings = new Dictionary<Type, List<IInvokeStategy>>();

        public void Bind<TSource, TTo>() where TTo : class, TSource
        {
            var typeSource = typeof (TSource);

            if (!bindings.ContainsKey(typeSource))
                bindings[typeSource] = new List<IInvokeStategy>();

            bindings[typeSource].Add(new TypeInvokeStrategy(typeof(TTo), this));
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
            return bindings[type].Select(b => b.Get());
        }

        public object Get(Type type)
        {
            return GetAll(type).Cast<object>().Single();
        }
    }
}
