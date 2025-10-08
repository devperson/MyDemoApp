using BestApp.Abstraction.Main.Platform;
using Common.Abstrtactions;
using DryIoc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BestApp.Abstraction.Main.UI;
using UnitTest.ViewModel.Impl;
using IntegrationTest.Impl;
using BestApp.ViewModels;
using UnitTest.InfrasImpl.Impl;
using BestApp.ViewModels.Base;
using BestApp.Abstraction.Common;
using Logging.Aspects;
using Mapster;
using MapsterMapper;
using UnitTest.Impl;
using BestApp.ViewModels.Movies;
using BestApp.Abstraction.Main.UI.Navigation;

namespace IntegrationTest.Base
{
    [TestClass]
    public class IoCAware
    {
        public static IContainer Container { get; set; }
        public ILoggingService Logger { get; set; }

        public IoCAware()
        {
            if (Container == null)
                Init();
        }

        [AssemblyInitialize]
        public static void BeforeAllTestsInAssembly(TestContext context)
        {

        }

        public void Init()
        {
            Container = new Container(
                  Rules.Default.With(FactoryMethod.ConstructorWithResolvableArgumentsIncludingNonPublic).WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Replace));

            RegisterTypes(Container);
        }

        private void RegisterTypes(IContainer container)
        {
            //register common
            var mapperConfig = new TypeAdapterConfig();
            container.RegisterInstance(mapperConfig);            
            container.Register<IMapper, Mapper>(Reuse.Singleton);
            container.Register<ILoggingService, MockAppLogging>();

            //register appService, infrastructure
            BestApp.Impl.Cross.Registerar.RegisterAppService(container, mapperConfig);
            BestApp.Impl.Cross.Registerar.RegisterInfrastructureService(container, mapperConfig);
            Container.Register<IConstants, ConstImpl>(Reuse.Singleton);            
            container.Register<IEventAggregator, EventAggregator>(Reuse.Singleton);

            //Register platforms            
            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);
            container.Register<IPopupAlert, MockPopup>(Reuse.Singleton);
            container.Register<IPageNavigationService, PageNavigationService>(Reuse.Singleton);
            container.Register<IPreferencesService, PreferenceService>(Reuse.Singleton);
            //mocked platforms            
            var mockPlatformError = new Mock<IPlatformErrorService> { DefaultValue = DefaultValue.Mock };            
            container.RegisterInstance(mockPlatformError.Object);

            //viewmodels
            container.Register<InjectedServices>();
            container.RegisterViewModel<MoviesPageViewModel>();
            container.RegisterViewModel<AddEditMoviePageViewModel>();

            Logger = container.Resolve<ILoggingService>();
            LogMethodsAttribute.LoggingService = Logger;
        }


        private ILoggingService logger;
        protected void LogMessage(string message)
        {
            if (logger == null)
                logger = Container.Resolve<ILoggingService>();
            logger.Log(message);
        }

        protected void LogWarning(string message)
        {
            if (logger == null)
                logger = Container.Resolve<ILoggingService>();
            logger.LogWarning(message);
        }

        protected void ThrowException(string message)
        {
            LogWarning(message);
            throw new System.Exception(message);
        }
    }

    public static class Extensions
    {
        public static void RegisterViewModel<T>(this IContainer container)
        {
            var type = typeof(T);
            var name = type.Name;
            PageNavigationService.RegisteredPageList.Add(name, type);
            container.Register(type, Reuse.Transient);//container.Register<T>(serviceKey: name);
        }
    }
}
