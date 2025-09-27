using BestApp.Abstraction.General.AppService.Services;
using BestApp.Impl.Shared.AppService.Products;
using DryIoc;
using Mapster;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.AppService
{
    internal class AppServRegisterar
    {
        internal static void RegisterTypes(IContainer container, TypeAdapterConfig config)
        {
            //register manual mapping
            //ASMapper.RegisterManualMapping(config);

            //register service impl
            container.Register<IProductService, ProductService>(Reuse.Singleton);
        }
    }
}
