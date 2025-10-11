using Base.Abstractions.AppService;
using BestApp.Abstraction.Main.AppService.Dto;

namespace BestApp.Abstraction.Main.AppService
{
    public interface IProductService
    {
        Task<Some<ProductDto>> Get(int productId);
        Task<Some<ProductDto>> Add(string name, int quantity, decimal cost);
        Task<Some<List<ProductDto>>> GetSome(int count, int skip);

    }
}
