using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.Infasructures.Exceptions
{
    public class AuthExpiredException : Exception
    {
        public AuthExpiredException() : base("user access token is expired") { }
    }
}
