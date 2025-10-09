using BestApp.Abstraction.Common.Events;
using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.Main.Infasructures.REST;
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
        public ProductRestService(Lazy<ILoggingService> loggingService, 
                                  Lazy<IAuthTokenService> authTokenService, 
                                  Lazy<IRestClient> restClient,
                                  Lazy<IEventAggregator> eventAggregator,
                                  RequestQueueList requestQueues) : base(loggingService, authTokenService, restClient, eventAggregator,requestQueues)
        {
        }

        public List<Product> GetAllProducts()
        {
            throw new NotImplementedException();
        }
    }
}
