using BestApp.Abstraction.Common;
using BestApp.Abstraction.Main.Infasructures;
using BestApp.Abstraction.Main.Platform;
using Common.Abstrtactions;
using DryIoc;
using Logging.Aspects;
using Mapster;
using MapsterMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Impl;
using UnitTest.InfrasImpl.Impl;

namespace UnitTest.InfrasImpl.Base
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

            var infrastructureService = Container.Resolve<IInfrastructureServices>();
            infrastructureService.Start().GetAwaiter().GetResult();
        }

        private static void RegisterTypes(IContainer container)
        {
            //register IConstants            
            Container.Register<IConstants, ConstImpl>(Reuse.Singleton);

            //register mapper
            var mapperConfig = new TypeAdapterConfig();
            container.RegisterInstance(mapperConfig);
            // Register Mapster's service
            container.Register<IMapper, Mapper>(Reuse.Singleton);

            //Register Common
            container.Register<ILoggingService, MockAppLogging>(Reuse.Singleton);
            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);
            container.Register<IPreferencesService, PreferenceService>(Reuse.Singleton);
            container.Register<IEventAggregator, EventAggregator>(Reuse.Singleton);
            LogMethodsAttribute.LoggingService = container.Resolve<ILoggingService>();

            BestApp.Impl.Cross.Registerar.RegisterInfrastructureService(container, mapperConfig);
            
        }
    }
}
