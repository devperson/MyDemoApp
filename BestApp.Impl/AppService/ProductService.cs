using BestApp.Abstraction.General.Infasructures;
using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.AppService.Services;
using BestApp.Abstraction.General.AppService.Dto;
using BestApp.Abstraction.General.AppService;
using Common.Abstrtactions;
using MapsterMapper;

namespace BestApp.Impl.Shared.AppService.Products
{
    internal class ProductService : IProductService
    {
        readonly IRepository<Product> productRepository;
        private readonly ILoggingService loggingService;
        private readonly IMapper mapper;

        public ProductService(IRepository<Product> productRepository, 
                              ILoggingService loggingService,
                              IMapper mapper)
        {
            this.productRepository = productRepository;
            this.loggingService = loggingService;
            this.mapper = mapper;
        }

        public Task<Some<ProductDto>> Get(int productId)
        {
            return Task.Run(() =>
            {
                try
                {
                    var product = this.productRepository.FindById(productId);
                    var dtoProduct = mapper.Map<ProductDto>(product);
                    return new Some<ProductDto>(dtoProduct);
                }
                catch (Exception ex)
                {
                    loggingService.TrackError(ex);
                    return new Some<ProductDto>(ex);
                }
            });
        }

        public Task<Some<ProductDto>> Add(string name, int quantity, decimal cost)
        {
            return Task.Run(() =>
            {
                try
                {
                    var product = Product.Create(name, quantity, cost);
                    this.productRepository.Add(product);

                    var dtoProduct = mapper.Map<ProductDto>(product);                    
                    return new Some<ProductDto>(dtoProduct);
                }
                catch (Exception ex)
                {
                    loggingService.TrackError(ex);
                    return new Some<ProductDto>(ex);
                }
            });            
        }

        public Task<Some<List<ProductDto>>> GetSome(int count, int skip)
        {
            return Task.Run(() =>
            {
                try
                {
                    var result = this.productRepository.Take(count, skip);
                    var dtoList = mapper.Map<List<ProductDto>>(result);
                    return new Some<List<ProductDto>>(dtoList);
                }
                catch (Exception ex)
                {
                    loggingService.TrackError(ex);
                    return new Some<List<ProductDto>>(ex);
                }
            });
        }
    }
}
