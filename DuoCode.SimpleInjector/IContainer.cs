using System;
using System.Collections;
using System.Collections.Generic;

namespace DuoCode.SimpleInjector
{
    public interface IContainer
    {
        T Get<T>() where T : class;
        IEnumerable<T> GetAll<T>() where T : class;
        IEnumerable GetAll(Type type);
        object Get(Type type);

        BindingResult Bind<TSource, TTo>() where TTo : class, TSource;
        BindingResult Bind<TSource>(TSource constant) where TSource : class;
        BindingResult Bind(Type source, Type to);
    }
}