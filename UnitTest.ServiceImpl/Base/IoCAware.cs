using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.AppService;
using BestApp.Abstraction.General.Infasructures;
using BestApp.Abstraction.General.Platform;
using BestApp.Impl.Cross.Common;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using BestApp.Impl.Cross.Infasructures;
using Common.Abstrtactions;
using DryIoc;
using Mapster;
using MapsterMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using UnitTest.ServiceImpl.Impl;
using BestApp.Impl.Cross.Infasructures.Repositories;

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

            RegisterTypes(Container);
        }

        private static void RegisterTypes(IContainer container)
        {
            //register mapper
            var config = new TypeAdapterConfig();
            container.RegisterInstance(config);
            // Register Mapster's service
            container.Register<IMapper, Mapper>(Reuse.Singleton);

            //Register Common
            container.Register<ILoggingService, MockAppLogging>(Reuse.Singleton);
            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);

            //register infrastructures            
            container.Register<IRepository<Product>, MockRepository<Product>>();


            //register appService
            BestApp.Impl.Cross.Registerar.RegisterAppService(container, config);            
        }
    }
}
