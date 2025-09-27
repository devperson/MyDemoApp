using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.AppService.Services;
using BestApp.Abstraction.General.Infasructures;
using BestApp.Impl.Cross.AppService;
using BestApp.Impl.Cross.Infasructures.Repositories;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using BestApp.Impl.Shared.AppService.Products;
using DryIoc;
using Mapster;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Infasructures
{
    internal class InfraRegisterar
    {
        internal static void RegisterTypes(IContainer container, TypeAdapterConfig config)
        {
            //register mapping
            config.NewConfig<Product, ProductTable>().TwoWays();
            //config.NewConfig<ProductTable, Product>();

            //register tables
            container.Register<IRepository<Product>, MemoryRepository<Product, ProductTable>>();
        }
    }
}
