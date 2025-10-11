using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.Main.AppService.Dto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Map
{
    internal static class AppMapper
    {
        internal static void RegisterMapping(TypeAdapterConfig config)
        {            
            config.NewConfig<Movie, MovieDto>().TwoWays();
            //config.NewConfig<Product, ProductDto>()
            //      .Map(dest => dest.Id, src => src.Id)
            //      .Map(dest => dest.Name, src => src.Name);

        }


        //public static ProductDto ToDto(this Product product)
        //{
        //    var dto = new ProductDto();
        //    dto.Id = product.Id;
        //    dto.Name = product.Name;
        //    dto.Active = product.Active;
        //    dto.Quantity = product.Quantity;
        //    dto.Modified = product.Modified;
        //    dto.Cost = product.Cost;
        //    dto.Created = product.Created;            
        //    return dto;
        //}

        //public static Product ToModel(this ProductDto dto)
        //{
        //    var model = new Product();
        //    model.Active = dto.Active;
        //    model.Quantity = dto.Quantity;
        //    model.Modified = dto.Modified;
        //    model.Cost = dto.Cost;
        //    model.Created = dto.Created;
        //    model.Id = dto.Id;
        //    model.Name = dto.Name;
        //    return dto;
        //}
    }
}
