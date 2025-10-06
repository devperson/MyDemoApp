using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.AppService.Dto;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Map
{
    internal class RepoMapper
    {
        internal static void RegisterMapping(TypeAdapterConfig config)
        {
            //register mapping
            config.NewConfig<Product, ProductTable>().TwoWays();
            config.NewConfig<MoviewTb, MoviewTb>().TwoWays();
        }
    }
}
