using BestApp.Abstraction.Main.Infasructures;
using BestApp.Impl.Cross.Infasructures.Repositories;
using DryIoc;
using Mapster;
using BestApp.Impl.Cross.Infasructures.REST;
using Base.Abstractions.Diagnostic;
using Base.Abstractions;
using Base.Impl.Common;
using Base.Infrastructures.Abstractions.Repository;
using Base.Infrastructures.Abstractions.REST;
using Base.Abstractions.REST;
using Base.Impl.REST;
using BestApp.Impl.Cross.Infasructures;


namespace Base.Impl
{
    public static class Registerar
    {
        public static void RegisterTypes(IContainer container)
        {
            //Register Common
            RegisterCommon(container);
            //register infrastructures           
            RegisterInfrastructureService(container);           
        }

        public static void RegisterCommon(IContainer container)
        {
            container.Register<IFileLoger, NLogFileLoger>(Reuse.Singleton);
            container.Register<ILoggingService, AppLoggingService>(Reuse.Singleton);
            container.Register<IMessagesCenter, SimpleMessageCenter>(Reuse.Singleton);
        }

        

        public static void RegisterInfrastructureService(IContainer container)
        {   
            //container.Register<ILocalDbInitilizer, SqliteDbInitilizer>(Reuse.Singleton);
            //container.Register<IRepository<Product>, SqliteRepository<Product, ProductTb>>(Reuse.Singleton);
            //container.Register<IRepository<Movie>, SqliteRepository<Movie, MovieTb>>(Reuse.Singleton);
            container.Register<IDatabaseInfo, DatabaseInfo>(Reuse.Singleton);            
            //rest service
            container.Register<IRestClient, RestClient>();
            container.Register<RequestQueueList>();
            container.Register<IAuthTokenService, AuthTokenService>(Reuse.Singleton);
            container.Register<IRestQueueService, RequestQueueList>(Reuse.Singleton);
        }
    }
}
