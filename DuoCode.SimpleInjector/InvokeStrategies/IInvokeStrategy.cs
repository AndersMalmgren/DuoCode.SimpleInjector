using System;

namespace DuoCode.SimpleInjector.InvokeStrategies
{
    internal interface IInvokeStrategy
    {
        object Get(Type requestedType);
    }
}
