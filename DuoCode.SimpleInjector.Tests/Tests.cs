using System;
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
    public sealed class When_getting_a_concrete_type
    {
        private ISimpleClass instance;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            instance = container.Get<SimpleClass>();
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
        private string message;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            try
            {
                container.Get<ISimpleClass>();
            }
            catch(Exception e)
            {
                message = e.Message;
                exception = true;
            }
        }

        [TestMethod]
        public void It_should_fail()
        {
            QUnit.equal(exception, true);
            QUnit.ok(message.Contains(typeof(ISimpleClass).FullName));
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
    public sealed class When_getting_type_twice_with_constructor_injected_dependency_and_transient_scope
    {
        private bool sameInstance;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            container.Bind<ISimpleClass, SimpleClass>();
            container.Bind<IDeepDepClass, DeepDepClass>();

            sameInstance = container.Get<IDeepDepClass>().SinmpleClass.GetHashCode() == container.Get<IDeepDepClass>().SinmpleClass.GetHashCode();
        }

        [TestMethod]
        public void It_should_have_created_new_objects()
        {
            QUnit.ok(!sameInstance);
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

    [Test]
    public sealed class When_getting_multiple_types_as_a_single
    {
        private bool exception;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            container.Bind<ISimpleClass, SimpleClass>();
            container.Bind<ISimpleClass, SimpleClassTwo>();

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
            QUnit.ok(exception);
        }
    }

    [Test]
    public sealed class When_getting_a_type_multiple_times
    {
        private bool sameInstance;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            container.Bind<ISimpleClass, SimpleClass>();


            sameInstance = container.Get<ISimpleClass>().GetHashCode() == container.Get<ISimpleClass>().GetHashCode();
        }

        [TestMethod]
        public void It_should_get_new_instance_each_time()
        {
            QUnit.ok(!sameInstance);
        }
    }

    [Test]
    public sealed class When_getting_a_constant_type_multiple_times
    {
        private bool sameInstance;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            container.Bind<ISimpleClass>(new SimpleClass());


            sameInstance = container.Get<ISimpleClass>().GetHashCode() == container.Get<ISimpleClass>().GetHashCode();
        }

        [TestMethod]
        public void It_should_get_same_instance_each_time()
        {
            QUnit.ok(sameInstance);
        }
    }

    [Test]
    public sealed class When_getting_a_none_constant_type_in_a_singleton_scope
    {
        private bool sameInstance;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();
            container
                .Bind<ISimpleClass, SimpleClass>()
                .InSingletonScope();

            sameInstance = container.Get<ISimpleClass>().GetHashCode() == container.Get<ISimpleClass>().GetHashCode();
        }

        [TestMethod]
        public void It_should_get_same_instance_each_time()
        {
            QUnit.ok(sameInstance);
        }
    }

    [Test]
    public sealed class When_getting_IContainer
    {
        private bool sameInstance;

        [TestSetup]
        public void Setup()
        {
            var container = new Container();

            sameInstance = container.GetHashCode() == container.Get<IContainer>().GetHashCode();

        }

        [TestMethod]
        public void It_should_get_same_instance()
        {
            QUnit.ok(sameInstance);
        }
    }
}