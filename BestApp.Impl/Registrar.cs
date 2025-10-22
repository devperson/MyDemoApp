using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.Main.Infasructures;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using BestApp.Impl.Cross.Infasructures.Repositories;
using DryIoc;
using Mapster;
using BestApp.Impl.Cross.Map;
using BestApp.Abstraction.Main.AppService;
using BestApp.Abstraction.Main.Infasructures.REST;
using BestApp.Impl.Cross.Infasructures.REST;
using BestApp.Impl.Cross.AppService;
using Base.Abstractions.Diagnostic;
using Base.Abstractions;
using Base.Impl.Common;
using Base.Infrastructures.Abstractions.Repository;
using BestApp.Impl.Cross.Infasructures;


namespace BestApp.Impl.Cross
{
    public static class Registrar
    {
        public static void RegisterTypes(IContainer container, TypeAdapterConfig mapperConfig)
        {
            Base.Impl.Registrar.RegisterTypes(container);
            //register infrastructures           
            RegisterInfrastructureService(container, mapperConfig, skipBase: true);
            //register appService
            RegisterAppService(container, mapperConfig);
        }

        
        public static void RegisterInfrastructureService(IContainer container, TypeAdapterConfig mapperConfig, bool skipBase = false)
        {
            //register infrastructures            
            if (!skipBase)
                Base.Impl.Registrar.RegisterInfrastructureService(container);
            //Sqlite            
            RepoMapper.RegisterMapping(mapperConfig);
            container.Register<ILocalDbInitilizer, SqliteDbInitilizer>(Reuse.Singleton);
            container.Register<IRepository<Movie>, SqliteRepository<Movie, MovieTb>>(Reuse.Singleton);
            //REST service
            container.Register<IMovieRestService, MovieRestService>(Reuse.Singleton);
            //common            
            container.Register<IInfrastructureServices, MyInfrastructureService>(Reuse.Singleton);//register base or derive from InfrastructureService and define own
            container.Register<IErrorTrackingService, MyErrorTrackingService>(Reuse.Singleton);
        }

        public static void RegisterAppService(IContainer container, TypeAdapterConfig mapperConfig)
        {            
            AppMapper.RegisterMapping(mapperConfig);
            container.Register<IMoviesService, MoviesService>(Reuse.Singleton);
        }

        
    }
}
