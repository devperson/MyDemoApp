using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.Infasructures.Exceptions
{
    public class RestApiException : Exception
    {
        public RestApiException(string message): base(message) { }
    }
}
