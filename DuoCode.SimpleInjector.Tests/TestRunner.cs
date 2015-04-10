using System;
using System.Linq;
using DuoCode.Runtime;
using QUnitJs;

// Set reflection-level (RTTI) for DuoCode compiler in order to find test methods using reflection
using UnitTest;

namespace DuoCode.SimpleInjector.Tests
{
    public static class TestRunner
    {
        static void Run() // HTML body.onload event entry point, see index.html
        {
            var assembly = typeof(TestRunner).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(TestAttribute), false).Count > 0)
                {
                    Console.WriteLine(type.FullName);

                    var methods = type.GetMethods();
                    
                    


                    var setup = methods.SingleOrDefault(m => m.GetCustomAttributes(typeof (TestSetup), false).Any());
                    var tests = methods.Where(m => m.GetCustomAttributes(typeof (TestMethodAttribute), false).Any());


                    foreach (var test in tests)
                    {
                        QUnit.test(type.FullName + "." + test.Name, () =>
                        {
                            var instance = type.GetConstructors()[0].Invoke(new object[0]);
                            if (setup != null)
                                setup.Invoke(instance, new object[0]);
                            test.Invoke(instance, new object[0]);

                        });
                        
                    }
                }
            }
        }
    }
}
