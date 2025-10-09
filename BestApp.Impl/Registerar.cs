using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.Main.Infasructures;
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
using BestApp.Abstraction.Main.AppService;
using BestApp.Abstraction.Main.Infasructures.REST;
using BestApp.Impl.Cross.Infasructures.REST;
using BestApp.Impl.Cross.AppService;
using IMessagesCenter = BestApp.Abstraction.Common.Events.IMessagesCenter;
using BestApp.Abstraction.Common.Events;


namespace BestApp.Impl.Cross
{
    public static class Registerar
    {
        public static void RegisterTypes(IContainer container, TypeAdapterConfig mapperConfig)
        {
            //Register Common
            RegisterCommon(container);
            //register infrastructures           
            RegisterInfrastructureService(container, mapperConfig);
            //register appService
            RegisterAppService(container, mapperConfig);
        }

        public static void RegisterCommon(IContainer container)
        {
            container.Register<IFileLoger, NLogFileLoger>(Reuse.Singleton);
            container.Register<ILoggingService, AppLoggingService>(Reuse.Singleton);
            container.Register<IMessagesCenter, SimpleMessageCenter>(Reuse.Singleton);
        }

        public static void RegisterAppService(IContainer container, TypeAdapterConfig mapperConfig)
        {
            AppMapper.RegisterMapping(mapperConfig);
            container.Register<IProductService, ProductService>(Reuse.Singleton);
            container.Register<IMoviesService, MoviesService>(Reuse.Singleton);
        }

        public static void RegisterInfrastructureService(IContainer container, TypeAdapterConfig mapperConfig)
        {
            //register infrastructures            
            //Sqlite            
            RepoMapper.RegisterMapping(mapperConfig);
            container.Register<ILocalDbInitilizer, SqliteDbInitilizer>(Reuse.Singleton);
            container.Register<IRepository<Product>, SqliteRepository<Product, ProductTb>>(Reuse.Singleton);
            container.Register<IRepository<Movie>, SqliteRepository<Movie, MovieTb>>(Reuse.Singleton);
            container.Register<IDatabaseInfo, DatabaseInfo>(Reuse.Singleton);            
            //rest service
            container.Register<IRestClient, RestClient>();
            container.Register<RequestQueueList>();
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
