using Base.Abstractions.AppService;
using Base.Abstractions.Diagnostic;
using Base.Abstractions.Messaging;
using Base.Abstractions.PlatformServices;
using Base.Abstractions.REST;
using Base.Abstractions.UI;
using Base.Aspect;
using Base.MVVM.Events;
using Base.MVVM.Navigation;
using BestApp.Abstraction.Main.AppService;
using BestApp.Abstraction.Main.AppService.Dto;
using BestApp.Abstraction.Main.Infasructures;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.Movies;
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
            //container.Register<IMessagesCenter, SimpleMessageCenter>(Reuse.Singleton);
            LogMethodsAttribute.LoggingService = container.Resolve<ILoggingService>();

            //infrastructures
            var mockInfraService = new Mock<IInfrastructureServices> { DefaultValue = DefaultValue.Mock };
            var mockEventAggregator = new Mock<IMessagesCenter> { DefaultValue = DefaultValue.Mock };
            //mock events
            mockEventAggregator.Setup(ea => ea.GetEvent<AuthErrorEvent>()).Returns(new AuthErrorEvent());
            mockEventAggregator.Setup(ea => ea.GetEvent<AppResumedEvent>()).Returns(new AppResumedEvent());
            mockEventAggregator.Setup(ea => ea.GetEvent<AppPausedEvent>()).Returns(new AppPausedEvent());
            container.RegisterInstance(mockInfraService.Object);
            container.RegisterInstance(mockEventAggregator.Object);

            //register app services
            var mockMovieService = new Mock<IMoviesService> { DefaultValue = DefaultValue.Mock };
            //get local data
            mockMovieService.Setup(x => x.GetListAsync(-1, 0, false)).ReturnsAsync(new Some<List<MovieDto>>(
            new List<MovieDto>
            {
                new MovieDto { Id = 1, Name = "Test movie1", Overview = "overview test1" }
            }));
            //get remote data
            mockMovieService.Setup(x => x.GetListAsync(-1, 0, true)).ReturnsAsync(new Some<List<MovieDto>>(
            new List<MovieDto>
            {
                new MovieDto { Id = 1, Name = "Test movie1", Overview = "overview test1" }
            }));
            mockMovieService.Setup(x => x.AddAsync("Test movie1", "test overview1", string.Empty)).ReturnsAsync(new Some<MovieDto>(new MovieDto()
            { 
                Id = 1, 
                Name = "Test movie1", 
                Overview = "test overview1"
            }));
            container.RegisterInstance(mockMovieService.Object);



            //Platform services
            container.Register<IPopupAlert, MockPopup>(Reuse.Singleton);
            var mockNavigationService = new Mock<IPageNavigationService> { DefaultValue = DefaultValue.Mock };
            var mockPlatformError = new Mock<IPlatformErrorService> { DefaultValue = DefaultValue.Mock };
            var mockSnackBar = new Mock<ISnackbarService> { DefaultValue = DefaultValue.Mock };
            var mockMeidaPicker = new Mock<IMediaPickerService> { DefaultValue = DefaultValue.Mock };
            var mockAlertDialog = new Mock<IAlertDialogService> { DefaultValue = DefaultValue.Mock };
            container.RegisterInstance(mockNavigationService.Object);
            container.RegisterInstance(mockPlatformError.Object);
            container.RegisterInstance(mockSnackBar.Object);
            container.RegisterInstance(mockMeidaPicker.Object);
            container.RegisterInstance(mockAlertDialog.Object);

            //viewmodels
            container.Register<InjectedServices>();
            container.Register<MoviesPageViewModel>();
            container.Register<AddEditMoviePageViewModel>();
        }
    }
}
