using System;

namespace DuoCode.SimpleInjector
{
    public interface IContainer
    {
        void Bind<TInterface, TType>() where TType : class, TInterface;
        T Get<T>() where T : class;
        object Get(Type type);
    }
}