using System.Collections.Generic;
using System.Linq;
using QUnitJs;
using UnitTest;

namespace DuoCode.SimpleInjector.Tests
{
    [Test]
    public sealed class When_binding_and_getting_to_a_single_type
    {
        private ISimpleClass instance;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            container.Bind<ISimpleClass, SimpleClass>();

            instance = container.Get<ISimpleClass>();
        }

        [TestMethod]
        public void It_should_create_class_correctly()
        {
            QUnit.ok(instance != null);
        }
    }

    [Test]
    public sealed class When_getting_a_abstract_none_bound_type
    {
        private bool exception;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            try
            {
                container.Get<ISimpleClass>();
            }
            catch
            {
                exception = true;
            }
        }

        [TestMethod]
        public void It_should_fail()
        {
            QUnit.equal(exception, true);
        }
    }

    [Test]
    public sealed class When_getting_type_with_constructor_injected_dependency
    {
        private IDeepDepClass instance;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            container.Bind<ISimpleClass, SimpleClass>();
            container.Bind<IDeepDepClass, DeepDepClass>();

            instance = container.Get<IDeepDepClass>();
        }

        [TestMethod]
        public void It_should_create_class_correctly()
        {
            QUnit.ok(instance.SinmpleClass.Member != null);
        }
    }

    [Test]
    public sealed class When_getting_multiple_types
    {
        private IEnumerable<ISimpleClass> instances;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            container.Bind<ISimpleClass, SimpleClass>();
            container.Bind<ISimpleClass, SimpleClassTwo>();

            instances = container.GetAll<ISimpleClass>();
        }

        [TestMethod]
        public void It_should_create_class_correctly()
        {
            QUnit.equal(2, instances.Count());
        }
    }

    [Test]
    public sealed class When_getting_multiple_types_with_constructor_injection
    {
        private IDeepClassWithCollection instance;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            container.Bind<ISimpleClass, SimpleClass>();
            container.Bind<ISimpleClass, SimpleClassTwo>();
            container.Bind<IDeepClassWithCollection, DeepClassWithCollection>();


            instance = container.Get<IDeepClassWithCollection>();
        }

        [TestMethod]
        public void It_should_create_class_correctly()
        {
            QUnit.equal(2, instance.SimpleClasses.Count());
            QUnit.ok(instance.SimpleClasses.Any(sc => sc.Member == "Foo"));
        }
    }
}