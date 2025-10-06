using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.Infasructures.REST
{
    public interface IRestClient
    {        
        Task<string> DoHttpRequest(RestMethod method, RestRequest restRequest);
    }
}
