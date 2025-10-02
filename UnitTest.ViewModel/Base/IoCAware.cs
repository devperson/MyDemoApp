using BestApp.Abstraction.General.AppService;
using BestApp.Abstraction.General.AppService.Dto;
using BestApp.Abstraction.General.Platform;
using BestApp.Abstraction.General.UI;
using Common.Abstrtactions;
using DryIoc;
using ImTools;
using Mapster;
using MapsterMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTest.ServiceImpl.Impl;

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
            var mockProductService = new Mock<IProductService> { DefaultValue = DefaultValue.Mock };
            mockProductService.Setup(x => x.GetSome(10, 0)).ReturnsAsync(new Some<List<ProductDto>>(
            new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Test prod1", Cost =5, Quantity=1 }
            }));
            mockProductService.Setup(x => x.Add("Test prod1", 1, 5)).ReturnsAsync(new Some<ProductDto>(new ProductDto()
            { Id = 1, Name = "Test prod1", Cost = 5, Quantity = 1 }));
            container.RegisterInstance(mockProductService.Object);

            //register ViewModel's required services
            var mockNavigationService = new Mock<IPageNavigationService> { DefaultValue = DefaultValue.Mock };
            var mockEventAggregator = new Mock<IEventAggregator> { DefaultValue = DefaultValue.Mock };
            var mockPopupService = new Mock<IPopupAlert> { DefaultValue = DefaultValue.Mock };
            var mockPlatformService = new Mock<IPlatformErrorService> { DefaultValue = DefaultValue.Mock };
            container.RegisterInstance(mockNavigationService.Object);
            container.RegisterInstance(mockEventAggregator.Object);
            container.RegisterInstance(mockPopupService.Object);
            container.RegisterInstance(mockPlatformService.Object);
            //await BestApp.Impl.Cross.Registerar.RegisterTypes(container);            
        }
    }
}
