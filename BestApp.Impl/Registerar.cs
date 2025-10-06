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
using BestApp.Abstraction.General.Infasructures.REST;
using BestApp.Impl.Cross.Infasructures.REST;
using BestApp.Impl.Cross.AppService;


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
            RegisterInfrastructureService(container, mapperConfig);
            //register appService
            RegisterAppService(container, mapperConfig);
        }

        public static void RegisterAppService(IContainer container, TypeAdapterConfig mapperConfig)
        {
            AppMapper.RegisterMapping(mapperConfig);
            container.Register<IProductService, ProductService>(Reuse.Singleton);
            container.Register<IMovieService, MovieService>(Reuse.Singleton);
        }

        public static void RegisterInfrastructureService(IContainer container, TypeAdapterConfig mapperConfig)
        {
            //register infrastructures            
            //Sqlite            
            RepoMapper.RegisterMapping(mapperConfig);
            container.Register<ILocalDbInitilizer, SqliteDbInitilizer>(Reuse.Singleton);
            container.Register<IRepository<Product>, SqliteRepository<Product, ProductTable>>(Reuse.Singleton);
            container.Register<IRepository<Movie>, SqliteRepository<Movie, MoviewTb>>(Reuse.Singleton);
            container.Register<IDatabaseInfo, DatabaseInfo>(Reuse.Singleton);            
            //rest service
            container.Register<IRestClient, RestClient>();
            container.Register<IAuthTokenService, AuthTokenService>(Reuse.Singleton);
            container.Register<IRestQueueService, RequestQueueList>(Reuse.Singleton);
            container.Register<IPorductRestService, ProductRestService>(Reuse.Singleton);
            container.Register<IMovieRestService, MovieRestService>(Reuse.Singleton);
            //common
            container.Register<IErrorTrackingService, ErrorTrackingService>(Reuse.Singleton);
            container.Register<IInfrastructureServices, InfrastructureService>(Reuse.Singleton);
        }
    }
}
