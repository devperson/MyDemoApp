using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.Infasructures;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using BestApp.Impl.Cross.Infasructures.Repositories;
using BestApp.Impl.Cross.AppService.Products;
using DryIoc;
using Mapster;
using MapsterMapper;
using BestApp.Impl.Cross.Map;
using Common.Abstrtactions;
using BestApp.Impl.Cross.Common;
using BestApp.Impl.Cross.Infasructures;
using BestApp.Abstraction.General.AppService;


namespace BestApp.Impl.Cross
{
    public static class Registerar
    {
        public static void RegisterTypes(IContainer container)
        {
            //register mapper
            var mapperConfig = new TypeAdapterConfig();
            container.RegisterInstance(mapperConfig);
            // Register Mapster's service
            container.Register<IMapper, Mapper>(Reuse.Singleton);

            //Register Common
            container.Register<IFileLoger, NLogFileLoger>(Reuse.Singleton);            
            container.Register<ILoggingService, AppLoggingService>(Reuse.Singleton);

            //register infrastructures            
            RepoMapper.RegisterMapping(mapperConfig);
            //container.Register<IRepository<Product>, MockRepository<Product, ProductTable>>();
            container.Register<IRepository<Product>, SqliteRepository<Product, ProductTable>>();
            container.Register<IDatabaseInfo, DatabaseInfo>(Reuse.Singleton);
            container.Register<IErrorTrackingService, ErrorTrackingService>(Reuse.Singleton);


            //register appService
            RegisterAppService(container, mapperConfig);
        }

        public static void RegisterAppService(IContainer container, TypeAdapterConfig mapperConfig)
        {
            AppMapper.RegisterMapping(mapperConfig);
            container.Register<IProductService, ProductService>(Reuse.Singleton);
        }
    }
}
