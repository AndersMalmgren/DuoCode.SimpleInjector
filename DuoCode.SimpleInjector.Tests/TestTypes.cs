using System;
using System.Collections.Generic;

namespace DuoCode.SimpleInjector.Tests
{
    public interface ISimpleClass
    {
        string Member { get; }
    }

    public class SimpleClass : ISimpleClass
    {
        public string Member { get { return "Foo"; } }
    }

    public class SimpleClassTwo : ISimpleClass
    {
        public string Member { get { return "Two"; } }
    }

    public interface IDeepDepClass
    {
        ISimpleClass SinmpleClass { get; }
    }

    public class DeepDepClass : IDeepDepClass
    {
        private readonly ISimpleClass sinmpleClass;

        public DeepDepClass(ISimpleClass sinmpleClass)
        {
            this.sinmpleClass = sinmpleClass;
        }

        public ISimpleClass SinmpleClass { get { return sinmpleClass; } }
    }

    public interface IDeepClassWithCollection
    {
        IEnumerable<ISimpleClass> SimpleClasses { get; }
    }

    public class DeepClassWithCollection : IDeepClassWithCollection
    {
        private readonly IEnumerable<ISimpleClass> colletion;

        public DeepClassWithCollection(IEnumerable<ISimpleClass> colletion)
        {
            this.colletion = colletion;
        }

        public IEnumerable<ISimpleClass> SimpleClasses { get { return colletion; } }
    }

    public class DeepClassWithAutoFactory
    {
        private ISimpleClass simpleClass;

        public DeepClassWithAutoFactory(Func<ISimpleClass> factory)
        {
            simpleClass = factory();
        }

        public ISimpleClass SimpleClass { get { return simpleClass; } }
    }

    public class DeepClassWithCollectionAutoFactory
    {
        private IEnumerable<ISimpleClass> simpleClasses;

        public DeepClassWithCollectionAutoFactory(Func<IEnumerable<ISimpleClass>> factory)
        {
            simpleClasses = factory();
        }

        public IEnumerable<ISimpleClass> SimpleClasses { get { return simpleClasses; } }
    }
}