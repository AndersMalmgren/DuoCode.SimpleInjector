using System;
using System.Collections;
using System.Collections.Generic;

namespace DuoCode.SimpleInjector
{
    public interface IContainer
    {
        void Bind<TInterface, TType>() where TType : class, TInterface;
        T Get<T>() where T : class;
        IEnumerable<T> GetAll<T>() where T : class;

        IEnumerable GetAll(Type type);
        object Get(Type type);
    }
}