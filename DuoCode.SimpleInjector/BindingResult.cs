using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuoCode.SimpleInjector.InvokeStrategies;
using DuoCode.SimpleInjector.LifetimeStrategies;

namespace DuoCode.SimpleInjector
{
    public class BindingResult
    {
        private readonly IInvokeStrategy invoker;
        private readonly Action<IInvokeStrategy> replaceInvoker;

        internal BindingResult(IInvokeStrategy invoker, Action<IInvokeStrategy> replaceInvoker)
        {
            this.invoker = invoker;
            this.replaceInvoker = replaceInvoker;
        }

        public void InSingletonScope()
        {
            replaceInvoker(new SingletonStrategy(invoker));
        }
    }
}
