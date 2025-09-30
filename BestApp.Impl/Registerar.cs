using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.AppService.Services;
using BestApp.Abstraction.General.Infasructures;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using BestApp.Impl.Cross.Infasructures.Repositories;
using BestApp.Impl.Shared.AppService.Products;
using DryIoc;
using Mapster;
using MapsterMapper;
using BestApp.Impl.Cross.Map;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross
{
    public static class Registerar
    {
        public static async Task RegisterTypes(IContainer container)
        {
            //register mapper
            var config = new TypeAdapterConfig();                        
            container.RegisterInstance(config);
            // Register Mapster's service
            container.Register<IMapper, Mapper>(Reuse.Singleton);

            //register repo            
            RepoMapper.RegisterMapping(config);
            //container.Register<IRepository<Product>, MockRepository<Product, ProductTable>>();
            container.Register<IRepository<Product>, SqliteRepository<Product, ProductTable>>();
            container.Register<DbConnectionInitilizer>(Reuse.Transient);
            
            //register appService
            AppMapper.RegisterMapping(config);
            container.Register<IProductService, ProductService>(Reuse.Singleton);


            //custom initializing
            var dbInitilizer = container.Resolve<DbConnectionInitilizer>();
            await dbInitilizer.RegisterDbConnection(container);
        }
    }
}
