using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.AppService.Dto;
using BestApp.Abstraction.General.AppService.Services;
using BestApp.Impl.Cross.AppService;
using BestApp.Impl.Cross.Infasructures;
using BestApp.Impl.Shared.AppService.Products;
using DryIoc;
using Mapster;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross
{
    public static class Registerar
    {
        public static void RegisterTypes(IContainer container)
        {
            //register mapper
            var config = new TypeAdapterConfig();                        
            container.RegisterInstance(config);
            // Register Mapster's service
            container.Register<IMapper, Mapper>(Reuse.Singleton);

            InfraRegisterar.RegisterTypes(container, config);
            AppServRegisterar.RegisterTypes(container, config);            
        }
    }
}
