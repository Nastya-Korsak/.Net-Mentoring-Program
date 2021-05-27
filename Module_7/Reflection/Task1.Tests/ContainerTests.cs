using NUnit.Framework;
using System.Reflection;
using Task1.Tests.Entities;

namespace Task1.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        private Container container;

        [SetUp]
        public void Setup()
        {
            container = new Container();
        }

        [Test]
        public void AddTypeT_SingleLoggerRegistration_ReturnLogger()
        {
            container.AddType(typeof(Logger));

            var logger = container.Get<Logger>();

            Assert.That(logger, Is.Not.Null);
            Assert.That(logger, Is.InstanceOf<Logger>());
        }

        [Test]
        public void AddTypeTandTBase_SingleICustomerDALRegistration_ReturnICustomerDAL()
        {
            container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerDAL = container.Get<ICustomerDAL>();

            Assert.That(customerDAL, Is.Not.Null);
            Assert.That(customerDAL, Is.InstanceOf<ICustomerDAL>());
        }

        [Test]
        public void AddTypeTandTBase_ImportConstructor_ReturnCustomerBLL()
        {
            container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
            container.AddType(typeof(Logger));
            container.AddType(typeof(CustomerBLL));

            var сustomerBLL = container.Get<CustomerBLL>();

            Assert.That(сustomerBLL, Is.Not.Null);
            Assert.That(сustomerBLL, Is.InstanceOf<CustomerBLL>());
        }

        [Test]
        public void AddTypeTandTBase_Import_ReturnCustomerBLL2()
        {
            container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
            container.AddType(typeof(Logger));
            container.AddType(typeof(CustomerBLL2));

            var сustomerBLL = container.Get<CustomerBLL2>();

            Assert.That(сustomerBLL, Is.Not.Null);
            Assert.That(сustomerBLL, Is.InstanceOf<CustomerBLL2>());
            Assert.That(сustomerBLL.CustomerDAL, Is.Not.Null);
            Assert.That(сustomerBLL.CustomerDAL, Is.InstanceOf<ICustomerDAL>());
            Assert.That(сustomerBLL.Logger, Is.Not.Null);
            Assert.That(сustomerBLL.Logger, Is.InstanceOf<Logger>());
        }

        [Test]
        public void GetT_NotRegistred_ThrowsError()
        {
            Assert.That(() => container.Get<ICustomerDAL>(), Throws.Exception);
        }

        [Test]
        public void GetT_NoDependencies_ThrowsError()
        {
            Assert.That(() => container.Get<CustomerBLL>(), Throws.Exception);
        }

        [Test]
        public void GetT_NotEnoughDependenciesForImportConstructor_ThrowsError()
        {
            container.AddType(typeof(Logger));
            Assert.That(() => container.Get<CustomerBLL>(), Throws.Exception);
        }

        [Test]
        public void GetT_NotEnoughDependenciesForImport_ThrowsError()
        {
            container.AddType(typeof(Logger));
            Assert.That(() => container.Get<CustomerBLL2>(), Throws.Exception);
        }

        [Test]
        public void AddAssembly_ImportConstructor_ReturnCustomerBLL()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var сustomerBLL = container.Get<CustomerBLL>();

            Assert.That(сustomerBLL, Is.Not.Null);
            Assert.That(сustomerBLL, Is.InstanceOf<CustomerBLL>());
        }

        [Test]
        public void AddAssembly_Import_ReturnCustomerBLL2()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var сustomerBLL = container.Get<CustomerBLL2>();

            Assert.That(сustomerBLL, Is.Not.Null);
            Assert.That(сustomerBLL, Is.InstanceOf<CustomerBLL2>());
            Assert.That(сustomerBLL.CustomerDAL, Is.Not.Null);
            Assert.That(сustomerBLL.CustomerDAL, Is.InstanceOf<ICustomerDAL>());
            Assert.That(сustomerBLL.Logger, Is.Not.Null);
            Assert.That(сustomerBLL.Logger, Is.InstanceOf<Logger>());
        }

        [Test]
        public void AddAssembly_GetLogger_ReturnLogger()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var сustomerBLL = container.Get<Logger>();

            Assert.That(сustomerBLL, Is.Not.Null);
            Assert.That(сustomerBLL, Is.InstanceOf<Logger>());
        }

        [Test]
        public void AddAssembly_GetICustomerDAL_ReturnICustomerDAL()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var сustomerBLL = container.Get<ICustomerDAL>();

            Assert.That(сustomerBLL, Is.Not.Null);
            Assert.That(сustomerBLL, Is.InstanceOf<ICustomerDAL>());
        }
    }
}
