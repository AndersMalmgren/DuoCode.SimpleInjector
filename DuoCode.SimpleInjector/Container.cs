using System;
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

        private readonly Dictionary<Type, Type> bindings = new Dictionary<Type, Type>();

        public void Bind<TInterface, TType>() where TType : class, TInterface
        {
            bindings[typeof(TInterface)] = typeof(TType);
        }

        public T Get<T>() where T : class
        {
            return Get(typeof(T)) as T;
        }

        public object Get(Type type)
        {
            var targetType = bindings.ContainsKey(type) ? bindings[type] : type;

            if (targetType.IsInterface || targetType.IsAbstract) throw new Exception("Can only inject concrete none abstract types");

            var constructor = targetType.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            var parameters = constructor.GetParameters()
                .Select(p => Get(p.ParameterType))
                .ToArray();

            var instance = constructor.Invoke(parameters);
            return instance;
        }
    }
}
