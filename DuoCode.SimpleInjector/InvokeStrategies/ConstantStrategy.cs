using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuoCode.SimpleInjector.InvokeStrategies
{
    internal class ConstantStrategy<T> : IInvokeStrategy
    {
        private readonly T constant;

        public ConstantStrategy(T constant)
        {
            this.constant = constant;
        }

        public object Get(Type requested)
        {
            return constant;
        }
    }
}
