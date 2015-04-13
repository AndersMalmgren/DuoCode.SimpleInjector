using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuoCode.SimpleInjector.InvokeStrategies;

namespace DuoCode.SimpleInjector.LifetimeStrategies
{
    internal class SingletonStrategy : IInvokeStrategy
    {
        private readonly IInvokeStrategy underlyingInvoker;
        private object instance;

        public SingletonStrategy(IInvokeStrategy underlyingInvoker)
        {
            this.underlyingInvoker = underlyingInvoker;
        } 

        public object Get()
        {
            return instance = instance ?? underlyingInvoker.Get();
        }
    }
}
