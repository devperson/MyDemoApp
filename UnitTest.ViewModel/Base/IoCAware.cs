using BestApp.Abstraction.General.AppService;
using BestApp.Abstraction.General.AppService.Dto;
using BestApp.Abstraction.General.Infasructures;
using BestApp.Abstraction.General.Platform;
using BestApp.Abstraction.General.UI;
using BestApp.ViewModels;
using BestApp.ViewModels.Base;
using Common.Abstrtactions;
using DryIoc;
using Mapster;
using MapsterMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTest.Impl;
using UnitTest.ViewModel.Impl;

namespace UnitTest.ViewModel.Base
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

            //register app services
            var mockMovieService = new Mock<IMovieService> { DefaultValue = DefaultValue.Mock };
            //get local data
            mockMovieService.Setup(x => x.GetList(-1, 0, false)).ReturnsAsync(new Some<List<MovieDto>>(
            new List<MovieDto>
            {
                new MovieDto { Id = 1, Name = "Test movie1", Overview = "overview test1" }
            }));
            //get remote data
            mockMovieService.Setup(x => x.GetList(-1, 0, true)).ReturnsAsync(new Some<List<MovieDto>>(
            new List<MovieDto>
            {
                new MovieDto { Id = 1, Name = "Test movie1", Overview = "overview test1" }
            }));
            mockMovieService.Setup(x => x.Add("Test movie1", "test overview1", string.Empty)).ReturnsAsync(new Some<MovieDto>(new MovieDto()
            { 
                Id = 1, 
                Name = "Test movie1", 
                Overview = "test overview1"
            }));
            container.RegisterInstance(mockMovieService.Object);                        

            //register ViewModel's required services
            var mockNavigationService = new Mock<IPageNavigationService> { DefaultValue = DefaultValue.Mock };
            var mockEventAggregator = new Mock<IEventAggregator> { DefaultValue = DefaultValue.Mock };            
            var mockPlatformService = new Mock<IPlatformErrorService> { DefaultValue = DefaultValue.Mock };
            var mockInfraService = new Mock<IInfrastructureServices> { DefaultValue = DefaultValue.Mock };
            container.RegisterInstance(mockNavigationService.Object);
            container.RegisterInstance(mockEventAggregator.Object);            
            container.RegisterInstance(mockPlatformService.Object);
            container.RegisterInstance(mockInfraService.Object);
            container.Register<InjectedServices>();
            container.Register<IPopupAlert, MockPopup>(Reuse.Singleton);


            container.Register<MainViewModel>();
            container.Register<CreateMovieViewModel>();
        }
    }
}
