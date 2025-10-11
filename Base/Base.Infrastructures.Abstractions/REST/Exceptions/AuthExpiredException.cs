using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.REST.Exceptions
{
    public class AuthExpiredException : Exception
    {
        public AuthExpiredException() : base("user access token is expired") { }
    }
}
