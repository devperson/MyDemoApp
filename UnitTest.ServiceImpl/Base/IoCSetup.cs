using Common.Abstrtactions;
using DryIoc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.ServiceImpl.Impl;

namespace UnitTest.ServiceImpl.Base
{
    [TestClass]
    public class IoCAware
    {
        public static IContainer Container { get; set; }

        public IoCAware()
        {
            if (Container == null)
                Init();
        }

        public static void Init()
        {
            Container = new Container(
                Rules.Default.With(FactoryMethod.ConstructorWithResolvableArgumentsIncludingNonPublic).WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Replace));


            RegisterTypes(Container);
        }

        private static void RegisterTypes(IContainer container)
        {
            BestApp.Impl.Cross.Registerar.RegisterTypes(container);
            container.Register<ILoggingService, MockAppLogging>(Reuse.Singleton);
        }
    }
}
