using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.Infasructures.Exceptions
{
    public class RestApiException : Exception
    {
        public RestApiException(string message): base(message) { }
    }
}
