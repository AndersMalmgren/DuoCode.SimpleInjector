using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
}