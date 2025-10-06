using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.Infasructures.REST;
using Common.Abstrtactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Infasructures.REST
{
    internal class ProductRestService : RestService, IPorductRestService
    {
        public ProductRestService(Lazy<ILoggingService> loggingService, Lazy<IAuthTokenService> authTokenService, Lazy<IRestClient> restClient) : base(loggingService, authTokenService, restClient)
        {
        }

        public List<Product> GetAllProducts()
        {
            throw new NotImplementedException();
        }
    }
}
