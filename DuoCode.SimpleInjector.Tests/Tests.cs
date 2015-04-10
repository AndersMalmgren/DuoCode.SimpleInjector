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
        public void It_should_fail()
        {
            QUnit.ok(instance.SinmpleClass.Member != null);
        }
    }
}