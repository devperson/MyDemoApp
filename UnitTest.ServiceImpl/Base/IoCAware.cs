using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.Main.Infasructures;
using BestApp.Abstraction.Main.PlatformServices;
using Common.Abstrtactions;
using DryIoc;
using Mapster;
using MapsterMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestApp.Impl.Cross.Infasructures.Repositories;
using UnitTest.Impl;
using BestApp.Abstraction.Main.Infasructures.REST;
using Moq;
using Logging.Aspects;
using BestApp.Abstraction.Common.Events;

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
            container.Register<IMessagesCenter, SimpleMessageCenter>(Reuse.Singleton);
            LogMethodsAttribute.LoggingService = container.Resolve<ILoggingService>();

            //register infrastructures            
            container.Register<IRepository<Product>, MockRepository<Product>>();
            container.Register<IRepository<Movie>, MockRepository<Movie>>();
            var mockMovieRestService = new Mock<IMovieRestService> { DefaultValue = DefaultValue.Mock };
            mockMovieRestService.Setup(x => x.GetMovieRestlist()).ReturnsAsync([
                new Movie 
                { 
                    Id = 1, 
                    Name = "remote movie sample", 
                    Overview = "some good overview", 
                    PosterUrl = string.Empty,
                }]);
            container.RegisterInstance<IMovieRestService>(mockMovieRestService.Object);


            //register appService
            BestApp.Impl.Cross.Registerar.RegisterAppService(container, config);            
        }
    }
}
