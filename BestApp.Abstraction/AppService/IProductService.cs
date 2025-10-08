using BestApp.Abstraction.Main.AppService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.AppService
{
    public interface IProductService
    {
        Task<Some<ProductDto>> Get(int productId);
        Task<Some<ProductDto>> Add(string name, int quantity, decimal cost);
        Task<Some<List<ProductDto>>> GetSome(int count, int skip);

    }
}
