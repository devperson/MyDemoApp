using BestApp.Abstraction.General.Infasructures;
using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.AppService.Dto;
using Common.Abstrtactions;
using MapsterMapper;
using BestApp.Abstraction.General.AppService;
using Logging.Aspects;

namespace BestApp.Impl.Cross.AppService.Products
{
    [LogMethods]
    internal class ProductService : IProductService
    {
        readonly Lazy<IRepository<Product>> productRepository;
        private readonly Lazy<ILoggingService> loggingService;
        private readonly Lazy<IMapper> mapper;

        public ProductService(Lazy<IRepository<Product>> productRepository,
                              Lazy<ILoggingService> loggingService,
                              Lazy<IMapper> mapper)
        {
            this.productRepository = productRepository;
            this.loggingService = loggingService;
            this.mapper = mapper;
        }

        public Task<Some<ProductDto>> Get(int productId)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var product = await this.productRepository.Value.FindById(productId);
                    var dtoProduct = mapper.Value.Map<ProductDto>(product);
                    return new Some<ProductDto>(dtoProduct);
                }
                catch (Exception ex)
                {
                    loggingService.Value.TrackError(ex);
                    return new Some<ProductDto>(ex);
                }
            });
        }

        public Task<Some<ProductDto>> Add(string name, int quantity, decimal cost)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var product = Product.Create(name, quantity, cost);
                    await this.productRepository.Value.Add(product);

                    var dtoProduct = mapper.Value.Map<ProductDto>(product);                    
                    return new Some<ProductDto>(dtoProduct);
                }
                catch (Exception ex)
                {
                    loggingService.Value.TrackError(ex);
                    return new Some<ProductDto>(ex);
                }
            });            
        }

        public Task<Some<List<ProductDto>>> GetSome(int count, int skip)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var result = await this.productRepository.Value.GetList(count, skip);
                    var dtoList = mapper.Value.Map<List<ProductDto>>(result);
                    return new Some<List<ProductDto>>(dtoList);
                }
                catch (Exception ex)
                {
                    loggingService.Value.TrackError(ex);
                    return new Some<List<ProductDto>>(ex);
                }
            });
        }
    }
}
