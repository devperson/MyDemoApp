using BestApp.Abstraction.General.Infasructures;
using Common.Abstrtactions;
using DryIoc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
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

        [AssemblyInitialize]
        public static void BeforeAllTestsInAssembly(TestContext context)
        {
           
        }


        public static void Init()
        {
            Container = new Container(
                  Rules.Default.With(FactoryMethod.ConstructorWithResolvableArgumentsIncludingNonPublic).WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Replace));

            RegisterTypes(Container).GetAwaiter().GetResult();
        }

        

        private static async Task RegisterTypes(IContainer container)
        {
            container.Register<ILoggingService, MockAppLogging>(Reuse.Singleton);
            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);
            await BestApp.Impl.Cross.Registerar.RegisterTypes(container);            
        }
    }
}
